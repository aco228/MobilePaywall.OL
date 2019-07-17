function Visual() {

  this.Holder = null;
  this._onEnter = null;
  this._onEscape = null;
  this.Country = '';

  // SUMMARY: Constructor
  this.init = function () {
    this.AddHolder();
    this.VisualCloseBtn();
  }

  // SUMMARY: Confirm box
  this.Confirm = function (text, confirm, deny, func) {
    var self = this;
    var id = this.ID("confirm");

    if (confirm == '' || typeof confirm === 'undefined') confirm = 'Confirm';
    if (deny == '' || typeof deny === 'undefined') deny = 'Close';

    var confirmHTML = "<div class=\"confirm_holder _visual_children\" id=\"" + id + "\" >" +
                           "<div class=\"confirm_holder_in\">" +
                             "<div class=\"confirm_holder_text\">" + text + "</div>" +
                             "<div class=\"confirm_holder_controls\">" +
                               "<div class=\"confirm_holder_btn confirm_btn_confirm\">" + confirm + "</div>" +
                               "<div class=\"confirm_holder_btn confirm_btn_deny _visual_close_btn\">" + deny + "</div>" +
                             "</div>" +
                             "<div style=\"clear:both\"></div>" +
                           "</div>" +
                         "</div>";

    var model = new VisualDisplayElementModel();
    model.ID = id;
    model.Html = confirmHTML;
    model.SetMargin = true;
    model.SuccessFunction = function () {
      $('#' + id + ' .confirm_btn_confirm').click(function () {
        self.Remove(id);
        if (typeof func !== 'undefined') func();
      });
    }

    this._onEscape = function () { self.Remove(id); }
    this._onEnter = function () { self.Remove(id); if (typeof func !== 'undefined') func(); }

    this.DisplayElement(model);
  }

  // SUMMARY: Alert box
  this.Alert = function (text, close, func) {
    var self = this;
    var id = this.ID("alert");

    if (typeof close === "undefined" || close == '') close = 'Close';

    // TODO: do this dynamic
    if (this.Country == 'IR') close = 'نزدیک';

    var alertHTML = '<div class="confirm_holder _visual_children" id="' + id + '" >' +
                      '<div class="confirm_holder_in">' +
                        '<div class="confirm_holder_text">' + text + '</div>' +
                        '<div class="confirm_holder_controls">' +
                          '<div class="confirm_holder_btn _visual_close_btn">' + close + '</div>' +
                        '</div>' +
                        '<div style="clear:both"></div>' +
                      '</div>' +
                    '</div>';

    var model = new VisualDisplayElementModel();
    model.ID = id;
    model.Html = alertHTML;
    model.SetMargin = true;

    this._onEnter = function () { self.Remove(id); }
    this._onEscape = function () { self.Remove(id); }

    this.DisplayElement(model);

    if (typeof func === 'function')
      $('#' + id).find('._visual_close_btn').click(function () { func(); });

    return id;
  }

  this.Wait = function (text) {
    var self = this;
    var id = this.ID("wait");

    var waitHtml = '<div class="confirm_holder _visual_children" id="' + id + '" >' +
                      '<div class="confirm_holder_in">' +
                        '<div class="confirm_holder_text">' + text + '</div>' +
                        '<div class="confirm_holder_controls">' +
                        '</div>' +
                        '<div style="clear:both"></div>' +
                      '</div>' +
                    '</div>';


    var model = new VisualDisplayElementModel();
    model.ID = id;
    model.Html = waitHtml;
    model.SetMargin = true;

    this._onEnter = function () { self.Remove(id); }
    this._onEscape = function () { self.Remove(id); }
    this.DisplayElement(model);
    return id;
  }

  // SUMMARY: Toast box
  this.Toast = function (text, time) {
    var self = this;
    var id = this.ID('toast');

    if (time == '' || typeof time === 'undefined') time = 3;

    var toastHTML = " <div class=\"toast_holder _visual_children noselect\" id=\"" + id + "\" > " +
                    " 	<div class=\"toast_holder_in\"> " +
                    " 		<div class=\"toast_holder_text\">" + text + "</div> " +
                    " 	</div> " +
                    " </div> ";

    var disposed = false;
    var model = new VisualDisplayElementModel();
    model.ID = id;
    model.Html = toastHTML;
    model.SetMargin = false;
    model.TransparentHolder = true;
    model.DisplayLogic = function (elem) {
      elem.css({ 'opacity': '1', 'bottom': '5px' });
    }
    model.SuccessFunction = function (elem) {
      elem.click(function () {
        if (!disposed) { self.Remove(id); disposed = true; }
      });
      setTimeout(function () {
        if (!disposed) { self.Remove(id); disposed = true; }
      }, time * 1000);
    }

    this.DisplayElement(model);
    return id;
  }



  // SUMMARY: Keyboard events sent from _system
  this.KeyboardEvent = function (event) {
    if (event == 'escape' && typeof this._onEscape == 'function') {
      this._onEscape(); this._onEscape = null;
    }
    else if (event == 'enter' && typeof this._onEnter == 'function') {
      this._onEnter(); this._onEnter = null;
    }
  }

  // SUMMARY: used for creating new holder from constructor
  this.AddHolder = function () {
    $('body').append('<div id="visual_holder" class="visual_holder_hide"></div>'); //style="opacity:0; visibility:hidden; display:none;"
    this.Holder = $('#visual_holder');
  }

  // SUMMARY: used before adding new element in holder
  this.InitiateHolder = function (model) {
    if (this.Holder.hasClass('visual_holder_hide'))
      this.Holder.removeClass('visual_holder_hide');

    if (model.TransparentHolder)
      this.Holder.css('background-color', 'rgba(0,0,0,0)');
    else
      this.Holder.css('background-color', 'rgba(0,0,0,.5)');

  }

  // SUMMARY: Used before removing element from holder
  this.DisposeHolder = function () {
    var self = this;

    if (self.Holder.find('._visual_children').length == 0 && !this.Holder.hasClass('visual_holder_hide'))
      self.Holder.addClass('visual_holder_hide');
  };

  // SUMMARY: Generic stuff for displaying elements
  this.DisplayElement = function (model) {
    if (typeof model !== 'object')
      return;

    var self = this;
    this.InitiateHolder(model);
    this.Holder.append(model.Html);
    var elem = $('#' + model.ID);

    if (model.SetMargin) {
      var margin_top = (this.Holder.outerHeight() / 2) - (elem.outerHeight() / 2);
      elem.css("margin-top", margin_top + "px");
    }

    if (typeof model.DisplayLogic === 'function')
      setTimeout(function () { model.DisplayLogic(elem); }, 10);
    else
      setTimeout(function () { elem.fadeIn(250); }, 10);


    if (typeof model.SuccessFunction === "function")
      model.SuccessFunction(elem);
  }

  // SUMMARY: Generic controler for closing visual elements
  this.VisualCloseBtn = function () {
    var self = this;
    this.Holder.on('click', '._visual_close_btn', function () {
      var elem = $(this).parent().parent().parent();
      self.DisposeHolder();
      self.Remove(elem.attr('id'));
    });
  };

  // SUMMARY: Removes element for visualHodler
  this.Remove = function (id) {
    var self = this;
    $('#' + id).hide(250, function () {
      $(this).remove();

      //if ($('#visual_holder').children().length == 0)
      //  $('#visual_holder').fadeOut(250);

      self.DisposeHolder();
    });

    this._onEnter = null;
    this._onEscape = null;
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

function VisualDisplayElementModel() {
  this.ID = '';
  this.Html = '';
  this.SetMargin = false;
  this.DisplayLogic = null;
  this.TransparentHolder = false;
  this.SuccessFunction = null;
}