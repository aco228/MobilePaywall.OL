function DatabaseManager(useOldConfiguration)
{
  this.configuration = null;
  this.window = null;
  this.dataContent = null;
  this.loading = null;
  this.inLoading = false;
  this._refreshTimer = null;
  this.useOldConfiguration = useOldConfiguration;

  // SUMMARY: Constructor
  this.init = function()
  {
    this.configuration = new DatabaseConfigurationManager();
    this.window = new WindowManager();
    this.catchKey();
    this.dataContent = $('#data_content');
    this.loading = $('#data_loading');
    //this.loading.css('opacity', '1');
    this.downloadCsv();
    this.collectPxids();
  }

  // SUMMARY: Catches keyboard keys
  this.catchKey = function ()
  {
    var self = this;
    document.onkeydown = function (evt) {
      var keyCode = evt ? (evt.which ? evt.which : evt.keyCode) : event.keyCode;
      if (keyCode == 13) { return self.onEnter(evt);  }
      else if (keyCode == 9) { return self.onTab(evt); }
      else if (keyCode == 27) { return self.onEscape(evt); }
      else if (keyCode == 113) { return self.openStatistic(evt); }
      else if (keyCode == 32) { return self.configuration.spaceKeyDown(evt); } // Space
      else if (keyCode == 81) { return self.openQuickReport(evt); } // Q
      else if (keyCode == 67) { return self.openCashflowStats(evt); } // C
      else if (keyCode == 68) { return self.openDatabaseReports(evt); } // D
      else if (keyCode == 80) { return self.configuration.paymentKeyDown(evt); } // p
      else if (keyCode == 82) { return self.configuration.paymentRequestKeyDown(evt); } // r
      else if (keyCode == 84) { return self.configuration.transactionKeyDown(evt); } // t
      else if (keyCode == 46) { return self.configuration.deleteKeyDown(evt); } // delete
      else return true;
    };

    $('#_mobileConfig').click(function (evt) { return self.onTab(evt); });
    $('#mobileFilter').click(function (evt) { return self.onEnter(evt); });
  }

  // SUMMARY: Event ESC (close all windows)
  this.onEscape = function ()
  {

    if (this.window.displayed)
      this.window.close();
    else if (this.configuration.displayed)
      this.configuration.close(true);

    return false;
  }

  // SUMMARY: Event Enter (load again)
  this.onEnter = function ()
  {
    this.configuration.close(true);
    this.load();
    return false;
  }
  
  // SUMMARY: Event Tab (open configuration window)
  this.onTab = function(event)
  {
    if (!this.configuration.displayed)
      this.configuration.open();
    else
      return this.configuration.close(false);

    return false;
  }

  // SUMMARY: Refresh automation, calls load method after adjusted time
  this.refreshManager = function()
  {
    var self = this;
    this.configuration.updateRefreshTime();
    clearTimeout(this._refreshTimer);

    if (this.configuration._refreshEnabled == 0)
      return;
    
    this._refreshTimer = setTimeout(function () {
      console.log('Refresh made...');

      if(!self.inLoading)
        self.load();

      self.refreshManager();
    }, this.configuration._refreshTime * 1000);
  }

  // SUMMARY: Double click on entry in main_table.. opens informations for that click
  this.doubleClick = function(elem)
  {
    var elem = $(elem);
    //var pguid = elem.attr('userSessionGuid');
    var uid = elem.attr('userSessionID');
    //var cguid = elem.attr('identificationSessionGuid');
    //var pxid = elem.find('.usPxid').text();
    //var date = elem.attr('date');
    //var ip = elem.find('.usIPAddress').text();

    var win = window.open('/report/'+uid, '_blank');
    win.focus();
  }

  // SUMMARY: Open statistic window ( on F2 click )
  this.openStatistic = function()
  {
    if(!this.window.displayed)
      this.window.open('statistic', 'timeline', this.configuration.params());
    else
      this.window.close();
  }

  // SUMMARY: Open cashflow reports (subscriptions) (on 'C' click)
  this.openCashflowStats = function(event)
  {
    if ($(':focus').is('input') || $(':focus').is('select'))
      return true;
    if (event.ctrlKey)
      return true;

    if (!this.window.displayed)
      this.window.open('statistic', 'cashflow', this.configuration.params());
    else
      this.window.close();

    return false;
  }
   
  // SUMMARY: Open Database report (on 'D' click)
  this.openDatabaseReports = function(event)
  {
    if ($(':focus').is('input') || $(':focus').is('select'))
      return true;
    if (event.ctrlKey)
      return true;

    if (!this.window.displayed)
      this.window.open('DatabaseReport', 'Index', this.configuration.params());
    else
      this.window.close();

    return false;
  }

  this.openQuickReport = function()
  {
    if ($(':focus').is('input') || $(':focus').is('select'))
      return true;

    if (!this.window.displayed)
      this.window.open("statistic", "QuickReport", this.configuration.params())
    else
      window.close();

    return false;
  }


  // SUMMARY: Loads data in main_table
  this._lastLoadingID = '';
  this.load = function()
  {
    //if (this.inLoading) return;
    var self = this;
    this.loading.css('opacity', '1');
    this.inLoading = true;
    this._lastLoadingID = _system.ID('load');
    var start = new Date();

    var url = this.useOldConfiguration ? 'Load' : 'LoadNew';
    this.call(url, this.configuration.params(), function (response) {
      self.inLoading = false;
      var time = new Date() - start;
      $('#database_time').html(time + 'ms');

      self.dataContent.html(response);
      self.loading.css('opacity', '0');

      var count = parseInt($('#tableData').attr('count')) - $('.tr_duplicate').length;
      $('#database_count').text(count);
      $('#database_lastUpdate').text($('#tableData').attr('created'));

      // call refresh manager only if we are not using sequental serach.. else, sequental search will call it
      if (self.configuration._useSequentalSearch == 0)
        self.refreshManager();
    });
  }

  // SUMMARY: Event for downloading .csv for data that is presented
  this.downloadCsv = function()
  {
    var self = this;
    $('#database_downloadCsv').click(function () {
      var win = window.open('/database/Csv' +  self.configuration.query());
      win.focus();
    });
  }

  this.collectPxids = function()
  {
    var self = this;
    $('#database_collectPxids').click(function () {
      
      var pxids = '';
      $('#tableData tr').each(function () {
        var pxid = $(this).find('.usPxid').text();
        if (typeof pxid !== 'undefined' && pxid != null && pxid != '')
          pxids += pxid + '\n';
      });

      if (!self.window.displayed)
        self.window.showData(pxids);
      else
        self.window.close();

    });
  }

  // SUMMARY: database shared ajax call
  this.call = function(url, data, succ_func)
  {
    _system.ajax('/database/' + url, data, 'POST', function (response) {
      if (typeof succ_func === 'function') succ_func(response);
    });
  }

  this.init();
}