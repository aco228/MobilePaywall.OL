function InformationsBlockManager(uid)
{
  this.init = function ()
  {
    this.openClose();
  }

  this.openClose = function()
  {
    var self = this;
    $('.informationTitle').click(function () {
      var parent = $(this).closest('.informationBlock');
      if (!parent.hasClass('informationBlockClosed'))
        parent.addClass('informationBlockClosed');
      else
      {
        parent.removeClass('informationBlockClosed');
        if( typeof parent.attr('isLoaded') === 'undefined')
        {
          parent.attr('isloaded', 'loading');
          parent.find('.informationTitleLoading').css('display', 'block');
          self.call(parent.attr('methodname'), { uid: uid }, function (response) {
            parent.find('.informationTitleLoading').css('display', 'none');
            parent.attr('isloaded', 'loaded');
            parent.find('.informationBody').html(response);
          });
        }
      }
    });
  }

  this.call = function (url, data, succ_func) {
    _system.ajax('/ClickInformations/' + url, data, 'POST', function (response) {
      if (typeof succ_func === 'function') succ_func(response);
    });
  }

  this.init();
}