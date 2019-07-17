
function WindowManager()
{
  this.elem = null;
  this.displayed = false;

  // SUMMARY: Constructor
  this.init = function()
  {
    this.elem = $('#window');
  }

  // SUMMARY: Open window
  this.open = function(controller, action, data)
  {
    if (this.displayed) return;
    var self = this;
    _manager.loading.css('opacity', '1');
    $('#main_table').addClass('main_table_blur');

    _system.ajax('/'+ controller + '/' + action, data, 'POST', function (response) {
      _manager.loading.css('opacity', '0');
      self.elem.html(response);

      self.elem.css('display', 'initial');
      self.displayed = true;
    });
  }

  // SUMMARY: Show data in text area
  this.showData = function(data)
  {
    if (this.displayed) return;
    var self = this;
    _manager.loading.css('opacity', '1');
    $('#main_table').addClass('main_table_blur');

    var response = '<textarea class="windowTextArea">'+data+'</textarea>';

    _manager.loading.css('opacity', '0');
    self.elem.html(response);

    self.elem.css('display', 'initial');
    self.displayed = true;
  }

  // SUMMARY: Close window
  this.close = function()
  {
    if (!this.displayed) return;
    this.elem.css('display', 'none');
    $('#main_table').removeClass('main_table_blur');
    this.elem.html('');
    this.displayed = false;
  }


  this.init();
}
