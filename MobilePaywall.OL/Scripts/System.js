function System()
{

  this.init = function()
  {

  }

  // SUMMARY: Shared ajax call for all
  this.ajax = function(url, data, method, succ_func)
  {
    $.ajax({
      url: url,
      data: data,
      method: method,
      success: function (response) { if (typeof succ_func === 'function') succ_func(response); },
      error: function() {}
    });
  }

  this.message = function(text, timeout)
  {
    if (typeof timeout === 'undefined')
      timeout = 500 + 900;
    else
      timeout += 500;

    var id = this.ID('msg');
    var html = "<div class=\"message\" id=\""+id+"\" style=\"bottom:-150px; opacity:0;\">"+text+"</div>";
    $('#data_messgeHolder').append(html);
    var bottom = 50;
    setTimeout(function () {
      $('#' + id).css({ 'bottom': bottom + 'px', 'opacity': '.9' });
      setTimeout(function () {
        $('#' + id).css({ 'bottom': '5px', 'opacity': '0' });
        setTimeout(function () {
          $('#' + id).remove();
        }, 500);
      }, timeout);
    }, 10);
  }

  // SUMMARY: Creates ID with given prefix (prefix_id)
  this.ID = function (prefix) {
    if (typeof prefix == undefined) prefix = ""; else prefix += "_";
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    for (var i = 0; i < 15; i++) text += possible.charAt(Math.floor(Math.random() * possible.length));
    return prefix + text;
  }




  this.init();
}