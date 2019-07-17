function Login()
{
  this.message = null;
  this.redirect = '/';

  this._init = function()
  {
    this.message = $('#message');
    this.servicePosition();
    this.containerPosition();

    this.submit();
    this.passwordOnEnter();
  }

  this.servicePosition = function()
  {
    var length = $('.service').length;
    var root = Math.ceil(Math.sqrt(length));

    var height = Math.floor($('#container').height() / root);
    var width = Math.floor($('#container').width() / root);

    console.log(length + ' ' + root);

    $('.service').each(function () {
      $(this).css({ 'height': height + 'px', 'width': width + 'px' });
    });
  }

  this.containerPosition = function()
  {
    var container = $('#container');
    var login_container = $('#login_container');

    var top = (container.height() / 2) - (login_container.height() / 2);
    var left = (container.width() / 2) - (login_container.width() / 2);

    login_container.css({ 'left': left + 'px', 'top': top + 'px' });
  }

  this.passwordOnEnter = function()
  {
    $('#input_password').keypress(function (e) {
      if (e.which == 13) {
        console.log('a');
        $('#login_submit').trigger('click');
      }
    });
  }

  this.submit = function()
  {
    var self = this;
    $('#login_submit').attr('inuse', 'false');
    $('#login_submit').click(function () {
      if ($(this).attr('inuse') == 'true')
        return;

      self.message.text('');
      var username = $('#input_username').val(); if (username == '') { self.message.text('You did not entered username.'); return; }
      var password = $('#input_password').val(); if (password == '') { self.message.text('You did not entered password.'); return; }

      if (username == '')
      {

      }
      
      var btn = $(this);
      var text = $(this).text();
      btn.attr('inuse', 'true');
      btn.text('Wait..');

      $.ajax({
        method:'POST',
        url: '/login/login',
        data: { username: username, password: password},
        success:function(response)
        {
          if (response.status)
            window.location = self.redirect;

          self.message.text(response.message);
          btn.attr('inuse', 'false');
          btn.text(text);
        }
      });

    });
  }

  this._init();
}