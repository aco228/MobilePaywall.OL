function ReportManager()
{
  this.paywallGuid = '';
  this.userSessionID = '';
  this.cashflowGuid = '';
  this.pxid = '';
  this.date = '';
  this.ip = '';
  this.locationData = {};

  this.init = function()
  {
    this.openCloseListener();
    this.refreshButtonListener();
  }

  this.loadClickInformations = function()
  {
    var elem = $('#clickBasicInformations');
    this.call2('GetClickInformations', { uid: this.userSessionID }, function (response) {
      elem.html(response);
    });
  }

  this.loadOLCachce = function()
  {
    var elem = $('#_cacheInformations');
    this.call2('GetOLInfo', { uid : this.userSessionID }, function (response) {
      elem.html(response);
    });
  }

  // SUMMARY: Load basic informations for click
  this.loadInformations = function () {
    var elem = $('#informations');
    if (elem.attr('init') == 'true' || elem.attr('loading') == 'true')
      return;
    var startTime = new Date();
    $('#informations_loading').show();
    elem.attr('loading', 'true');

    $('#informations_loading').show();
    this.call('GetInformations', { SessionID: this.userSessionID, DateFrom: this.date }, function (response) {
      var time = new Date() - startTime;
      $('#informations_loadingTime').text(time);
      $('#informations_loadingTime').css('display', 'block');

      elem.attr('loading', 'false');
      elem.attr('init', 'true');

      $('#informations_loading').hide();
      $('#informations_body').html(response);
    });
  }

  // SUMMARY: Load basic informations for click
  this.loadNewInformations = function () {
    var elem = $('#newInformations');
    if (elem.attr('init') == 'true' || elem.attr('loading') == 'true')
      return;
    var startTime = new Date();
    $('#newInformations_loading').show();
    elem.attr('loading', 'true');

    $('#newInformations_loading').show();
    this.call2('GetInformations', { uid: this.userSessionID }, function (response) {
      var time = new Date() - startTime;
      $('#newInformations_loadingTime').text(time);
      $('#newInformations_loadingTime').css('display', 'block');

      elem.attr('loading', 'false');
      elem.attr('init', 'true');

      $('#newInformations_loading').hide();
      $('#newInformations_body').html(response);
    });
  }
  
  // SUMMARY: Load paywall logs on regular way
  this.loadPaywall = function ()
  {
    var elem = $('#paywall');
    if (elem.attr('loading') == 'true')
      return;

    var self = _manager;
    var startTime = new Date();
    $('#paywall_loading').show();
    elem.attr('loading', 'true');

    $('#paywall_loading').show();
    self.call('GetPaywallLog', { SessionGuid: self.paywallGuid, SessionID: self.userSessionID, DateFrom: self.date }, function (response) {
      var time = new Date() - startTime;
      $('#paywall_loadingTime').text(time);
      $('#paywall_loadingTime').css('display', 'block');

      $('#paywall_loading').hide();
      $('#paywall_body').html(response);

      elem.attr('loading', 'false');
      elem.attr('init', 'true');

      $('#paywall_body pre code').each(function (i, block) {
        hljs.highlightBlock(block);
      });
    });
  }

  // SUMMARY: Load paywall new fast method
  this.loadPaywall2 = function()
  {
    var elem = $('#paywall2');
    if (elem.attr('loading') == 'true')
      return;

    var self = _manager;
    var startTime = new Date();
    $('#paywall2_loading').show();
    elem.attr('loading', 'true');

    $('#paywall2_loading').show();
    self.call('GetPaywallLog2', { SessionGuid: self.paywallGuid, SessionID: self.userSessionID, DateFrom: self.date }, function (response) {
      var time = new Date() - startTime;
      $('#paywall2_loadingTime').text(time);
      $('#paywall2_loadingTime').css('display', 'block');

      $('#paywall2_loading').hide();
      $('#paywall2_body').html(response);

      elem.attr('loading', 'false');
      elem.attr('init', 'true');

      $('#paywall2_body pre code').each(function (i, block) {
        hljs.highlightBlock(block);
      });
    });
  }
  
  // SUMMARY: Load paywall new fast method (from Log or backup database)
  this.loadPaywallFromLogDatabase = function () {
    var elem = $('#paywall2');
    if (elem.attr('loading') == 'true')
      return;

    var self = _manager;
    var startTime = new Date();
    $('#paywall2_loading').show();
    elem.attr('loading', 'true');

    $('#paywall2_loading').show();
    self.call('GetPaywallLogDatabase', { SessionGuid: self.paywallGuid, SessionID: self.userSessionID, DateFrom: self.date }, function (response) {
      var time = new Date() - startTime;
      $('#paywall2_loadingTime').text(time);
      $('#paywall2_loadingTime').css('display', 'block');

      $('#paywall2_loading').hide();
      $('#paywall2_body').html(response);

      elem.attr('loading', 'false');
      elem.attr('init', 'true');

      $('#paywall2_body pre code').each(function (i, block) {
        hljs.highlightBlock(block);
      });
    });
  }

  // SUMMARY: Load UserHttpRequest from database
  this.loadRequests = function ()
  {
    var elem = $('#requests');
    if ( elem.attr('loading') == 'true')
      return;
    
    var self = _manager;
    var startTime = new Date();
    elem.attr('loading', 'true');
    $('#requests_loading').show();

    $('#requests_loading').show();
    self.call('GetUserHttpReqeusts', { SessionID: self.userSessionID, DateFrom: self.date }, function (response) {
      var time = new Date() - startTime;
      $('#requests_loadingTime').text(time);
      $('#requests_loadingTime').css('display', 'block');

      elem.attr('init', 'true');
      elem.attr('loading', 'false');

      $('#requests_loading').hide();
      $('#requests_body').html(response);
    });
  }

  // SUMMARY: Load cashflow logs from Cashlflow database
  this.loadCashflow = function () {
    var elem = $('#cashflow');
    if (elem.attr('loading') == 'true')
      return;

    var self = _manager;
    var startTime = new Date();

    elem.attr('loading', 'true');
    $('#cashflow_loading').show();

    self.call('GetCashflowLog', { SessionID: self.userSessionID, DateFrom: self.date }, function (response) {
      var time = new Date() - startTime;
      $('#cashflow_loadingTime').text(time);
      $('#cashflow_loadingTime').css('display', 'block');

      $('#cashflow_loading').hide();
      $('#cashflow_body').html(response);

      elem.attr('loading', 'false');
      elem.attr('init', 'true');

      $('#cashflow_body pre code').each(function (i, block) { hljs.highlightBlock(block); });

    });
  }

  // SUMMARY: Load cashflow logs from Cashlflow database
  this.loadCashflowNew = function () {
    var elem = $('#cashflowNew');
    if (elem.attr('loading') == 'true')
      return;

    var self = _manager;
    var startTime = new Date();

    elem.attr('loading', 'true');
    $('#cashflowNew_loading').show();

    self.call('GetCashflowNewLog', { SessionID: self.userSessionID, DateFrom: self.date }, function (response) {
      var time = new Date() - startTime;
      $('#cashflowNew_loadingTime').text(time);
      $('#cashflowNew_loadingTime').css('display', 'block');

      $('#cashflowNew_loading').hide();
      $('#cashflowNew_body').html(response);

      elem.attr('loading', 'false');
      elem.attr('init', 'true');

      $('#cashflow_body pre code').each(function (i, block) { hljs.highlightBlock(block); });

    });
  }

  this.loadCallback = function()
  {
    var elem = $('#callback');
    if (elem.attr('init') == 'true' || elem.attr('loading') == 'true')
      return;

    var self = _manager;
    var startTime = new Date();

    elem.attr('loading', 'true');
    elem.find('.block_loading').show();

    self.call('GetCallbackLog', { SessionID: self.userSessionID }, function (response) {
      var time = new Date() - startTime;
      elem.find('.block_loadTime').text(time);
      elem.find('.block_loadTime').css('display', 'block');
      elem.find('.block_loading').hide();
      elem.find('.block_body').html(response);

      elem.attr('loading', 'false');
      elem.attr('init', 'true');

      elem.find('.block_body').find('code').each(function (i, block) { hljs.highlightBlock(block); });
    });
  }

  // SUMMARY: Load location on google maps to
  this.loadLocation = function()
  {
    var elem = $('#location');
    if (elem.attr('init') == 'true' || elem.attr('loading') == 'true')
      return;

    var self = _manager;
    var startTime = new Date();
    var url = 'http://ip-api.com/json/' + self.ip;
    $('#location_loading').show();
    elem.attr('loading', 'true');

    $.ajax({
      method: 'GET', url: url,
      success:function(response)
      {
        var time = new Date() - startTime;
        $('#location_loadingTime').text(time);
        $('#location_loadingTime').css('display', 'block');
        $('#location_loading').hide();
        elem.attr('init', 'true');
        elem.attr('loading', 'false');

        self.locationData = { lat: response.lat, lng: response.lon };
        $('body').append('<script async defer src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBBdaeVjQG6gye-El9YsXJEkAF7YOeMGII&callback=initMap" type="text/javascript"></script>');

      }
    });

  }

  // SUMMARY: Load overlay logs from Clickenzi database
  this.loadOverlay = function ()
  {
    var elem = $('#overlay');
    if (elem.attr('loading') == 'true')
      return;
    
    var self = _manager;
    var startTime = new Date();
    $('#overlay_loading').show();
    elem.attr('loading', 'true');

    self.call('GetOverlayLogs', { SessionID: self.userSessionID, DateFrom: self.date }, function (response) {
      var time = new Date() - startTime;
      $('#overlay_loadingTime').text(time);
      $('#overlay_loadingTime').css('display', 'block');

      elem.attr('loading', 'false');
      elem.attr('init', 'true');

      $('#overlay_loading').hide();
      $('#overlay_body').html(response);
    });
  }

  this.openCloseListener = function()
  {
    $('body').on('click', '.openclose', function () {
      var elem = $(this).parent();
      var base = 'document';

      if (elem.hasClass('block')) base = 'block';

      if (elem.hasClass(base + '_closed'))
      {
        elem.removeClass(base + '_closed');
        if (typeof elem.attr('_onclick') !== 'undefined')
          eval("_manager." + elem.attr('_onclick'))();
      }
      else
        elem.addClass(base + '_closed');
    });
  }

  this.refreshButtonListener = function()
  {
    $('body').on('click', '.block_menu_item', function () {
      var func = $(this).attr("_onclick");
      if (typeof func !== 'undefined')
      {
        console.log(func);
        eval("_manager." + func)();
      }
    });
  }

  // SUMMARY: database shared ajax call
  this.call = function (url, data, succ_func) {
    _system.ajax('/database/' + url, data, 'POST', function (response) {
      if (typeof succ_func === 'function') succ_func(response);
    });
  }

  this.call2 = function (url, data, succ_func) {
    _system.ajax('/DatabaseReport/' + url, data, 'POST', function (response) {
      if (typeof succ_func === 'function') succ_func(response);
    });
  }

  this.init();
}