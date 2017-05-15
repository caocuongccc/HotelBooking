var extensionJS = {
    appPath: '',
    getVirtualPath: function (path) {
        if (extensionJS.appPath != '/') {
            return extensionJS.appPath + path;
        }
        else return path;
    },
}

var MessageBox = {
    Show: function (message, caption, messageBoxButton, messageBoxIcon, functionCallback) {
        var iconUrl = '';
        switch (messageBoxIcon) {
            case MessageBoxIcons.Warning:
                iconUrl = extensionJS.getVirtualPath('/images/warning.png');
                break;

            case MessageBoxIcons.Confirm:
                iconUrl = extensionJS.getVirtualPath('/images/information.png');
                break;

            case MessageBoxIcons.Error:
                iconUrl = extensionJS.getVirtualPath('/images/error.png');
                break;

            case MessageBoxIcons.Information:
                iconUrl = extensionJS.getVirtualPath('/images/information.png');
                break;

            case MessageBoxIcons.Question:
                iconUrl = extensionJS.getVirtualPath('/images/information.png');
                break;

            default:
                iconUrl = extensionJS.getVirtualPath('/images/information.png');
                break;
        }
        var buttons;
        var functions = [];
        switch (messageBoxButton) {
            case MessageBoxButtons.OK:
                buttons = '<button type="button" class="btn btn-primary">OK</button>';
                functions = [function () { MessageBox.Callback(DialogResults.OK, functionCallback); }];
                break;
            case MessageBoxButtons.OKCancel:
                buttons = '<button type="button" class="btn btn-primary">OK</button><button type="button" class="btn btn-default">Cancel</button>';
                functions = [function () { MessageBox.Callback(DialogResults.OK, functionCallback); },
                    function () { MessageBox.Callback(DialogResults.Cancel, functionCallback); }
                ];

                break;
            case MessageBoxButtons.AbortRetryIgnore:
                buttons = '<button type="button" class="btn btn-default">Abort</button><button type="button" class="btn btn-primary">Retry</button><button type="button" class="btn btn-default">Ignore</button>';
                functions = [
                    function () { MessageBox.Callback(DialogResults.Abort, functionCallback); },
                    function () { MessageBox.Callback(DialogResults.Retry, functionCallback); },
                    function () { MessageBox.Callback(DialogResults.Ignore, functionCallback); }
                ]
                break;
            case MessageBoxButtons.YesNoCancel:
                buttons = '<button type="button" class="btn btn-primary">Yes</button><button type="button" class="btn btn-default">No</button><button type="button" class="btn btn-default">Cancel</button>';
                functions = [
                    function () { MessageBox.Callback(DialogResults.Yes, functionCallback); },
                    function () { MessageBox.Callback(DialogResults.No, functionCallback); },
                    function () { MessageBox.Callback(DialogResults.Cancel, functionCallback); }
                ]
                break;
            case MessageBoxButtons.YesNo:
                buttons = '<button type="button" class="btn btn-primary">Yes</button><button type="button" class="btn btn-default">No</button>';
                functions = [
                   function () { MessageBox.Callback(DialogResults.Yes, functionCallback); },
                   function () { MessageBox.Callback(DialogResults.No, functionCallback); }
                ];
                break;
            case MessageBoxButtons.RetryCancel:
                buttons = '<button type="button" class="btn btn-primary">Retry</button><button type="button" class="btn btn-default">Cancel</button>';
                functions = [
                   function () { MessageBox.Callback(DialogResults.Retry, functionCallback); },
                   function () { MessageBox.Callback(DialogResults.Cancel, functionCallback); }
                ];
                break;
        }

        var div = '<div class="modal-body"><div class="media"><div class="media-left"><img class="media-object" src="' + iconUrl + '"></div><div class="media-body">' + caption + '</div></div></div><div class="modal-footer">' + buttons + '</div>';
        PopupDialog.ShowHtml({
            html: div, title: message, closeable: false,
            openCallback: function (m) {
                for (var i = 0; i < functions.length; i++) {
                    $($(m).find('.modal-footer button')[i]).click(functions[i]);
                }
            }
        });
    },
    Callback: function (dialogResult, functionCallback) {
        MessageBox.Close();
        if (functionCallback) {
            functionCallback(dialogResult);
        }
    },
    Close: function () {
        PopupDialog.Close();
    }
}
var MessageBoxButtons = {
    OK: 0,
    OKCancel: 1,
    AbortRetryIgnore: 2,
    YesNoCancel: 3,
    YesNo: 4,
    RetryCancel: 5,
}

var MessageBoxIcons = {
    Warning: "Warning",
    Confirm: "Confirm",
    Error: "Error",
    Information: "Information",
    Question: "Question"
}

var DialogResults = {
    OK: "OK",
    Cancel: "Cancel",
    Abort: "Abort",
    Retry: "Retry",
    Ignore: "Ignore",
    Yes: "Yes",
    No: "No"
}

var PopupDialog = {
    Stack: [],
    Result: null,
    Show: function (options) {
        options = options || {};
        if (!options.url) {
            throw 'url cannot be null';
        }
        $.ajax({
            url: extensionJS.getVirtualPath(options.url),
            type: "GET",
            dataType: "html",
        }).done(function (responseText) {
            options.html = responseText;
            PopupDialog.ShowHtml(options);
        });
    },
    Close: function () {
        var dialog = PopupDialog.Stack[PopupDialog.Stack.length - 1];
        $(dialog).modal('hide');
    },
    CloseAll: function () {
        while (PopupDialog.Stack.length > 0) {
            PopupDialog.Close();
        }
    },
    ShowHtml: function (options) {
        options = options || {};
        var html = options.html || '',
            title = options.title || '',
            openCallback = options.openCallback,
            closeCallback = options.closeCallback,
            closeable = options.closeable,
                size = options.size || 'medium';
        var sc = size == 'medium' ? '' : (size == 'large' ? 'modal-lg' : (size == 'small' ? 'modal-sm' : (size == 'auto' ? 'width-auto' : '')))
        if (closeable == null) closeable = true;
        //if (title == null) title = '';
        var div = $('#divDialog' + PopupDialog.Stack.length);
        if (div.length == 0) {
            div = "<div id='divDialog" + PopupDialog.Stack.length + "'  class='modal fade custom-dialog' tabindex='-1' role='dialog'></div>";
        }
        div = $(div).html('<div class="modal-dialog ' + sc + '" role="document"><div class="modal-content"></div></div>');
        html = (title ? '<div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button><h4 class="modal-title">' + title + '</h4></div>' : '') + html;
        $(div).find('.modal-content').html(html);
        $(div).modal({ backdrop: closeable });
        $(div).on('hide.bs.modal', function (e) {
            PopupDialog.Stack.pop();
            if (closeCallback) {
                closeCallback();
            }
            $(this).unbind();
        });
        $('body').addClass('modal-open');
        $(div).on('shown.bs.modal', function (e) {
            PopupDialog.Stack.push($(this));
            if (size == 'auto') {
                $(this).css('display', 'flex');
            }
            if (openCallback) {
                openCallback($(this));
            }
        });
    }
}