// -----------------------------------------------------------------
// File:    IndexHTML.cs
// Author:  mouguangyi
// Date:    2017.01.22
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Framework
{
    static class IndexHTML
    {
        public const string CONTENT = @"

<!DOCTYPE html>
<html lang='zh-CN'>
  <head>
    <meta charset='utf-8' >
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1'>
    <!-- 上述3个meta标签* 必须*放在最前面，任何其他内容都* 必须*跟随其后！ -->
    <title>GameBox Console</title>

    <!-- Bootstrap -->
    <link href='https://cdn.bootcss.com/bootstrap/3.3.7/css/bootstrap.min.css' rel='stylesheet'>
    <link href='https://cdn.bootcss.com/bootstrap/3.3.7/css/bootstrap-theme.min.css' rel='stylesheet'>

    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src='https://cdn.bootcss.com/html5shiv/3.7.3/html5shiv.min.js'></script>
      <script src='https://cdn.bootcss.com/respond.js/1.4.2/respond.min.js'></script>
    < ![endif]-- >
  </head>
  <body>
    
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src='https://cdn.bootcss.com/jquery/1.12.4/jquery.min.js'></script>
    <!--Include all compiled plugins (below), or include individual files as needed -->
    <script src='https://cdn.bootcss.com/bootstrap/3.3.7/js/bootstrap.min.js'></script>
    <script>
    var commandIndex = -1;
    var hash = null;

    function scrollBottom()
    {
        $('#output').scrollTop($('#output')[0].scrollHeight);
    }

    function runCommand(command)
    {
        scrollBottom();
        $.get(command, function(data, status) {
            updateConsole(function() {
                updateCommand(commandIndex - 1);
            });
        });
        resetInput();
    }

    function updateConsole(callback)
    {
        $.get('console/output', function(data, status) {
            // Check if we are scrolled to the bottom to force scrolling on update
            var output = $('#output');
            output.empty();
            output.append(String(data));
            shouldScroll = (output[0].scrollHeight - output.scrollTop()) == output.innerHeight();
            if (callback) callback();
            if (shouldScroll) scrollBottom();
        });
    }

    function resetInput()
    {
        commandIndex = -1;
        $('#input').val('');
    }

    function previousCommand()
    {
        updateCommand(commandIndex + 1);
    }

    function nextCommand()
    {
        updateCommand(commandIndex - 1);
    }

    function updateCommand(index)
    {
        // Check if we are at the defualt index and clear the input
        if (index < 0) {
            resetInput();
            return;
        }

        $.get('console/commandHistory?index=' + index, function(data, status) {
            if (data) {
                commandIndex = index;
                $('#input').val(String(data));
            }
        });
    }

    function complete(command)
    {
        $.get('console/complete?command=' + command, function(data, status) {
            if (data) {
                $('#input').val(String(data));
            }
        });
    }

    // Poll to update the console output
    window.setInterval(function() { updateConsole(null) }, 500);
    </script>
    <nav class='navbar navbar-inverse navbar-fixed-top'>
        <div class='container'>
            <div class='navbar-header'>
                <button type='button' class='navbar-toggle collapsed' data-toggle='collapse' data-target='#navbar' aria-expanded='false' aria-controls='navbar'>
                    <span class='sr-only'>Toggle navigation</span>
                    <span class='icon-bar'></span>
                    <span class='icon-bar'></span>
                    <span class='icon-bar'></span>
                </button>
            <a class='navbar-brand' href='#'>GameBox</a>
            </div>
            <div id='navbar' class='navbar-collapse collapse'>
                <ul class='nav navbar-nav'>
                    <li class='active'><a href='#'>Console</a></li>
                    <li><a href='#statics'>Statics</a></li>
                </ul>
            </div><!--/.nav-collapse -->
        </div>
    </nav>
    <div class='container theme-showcase' role='main'>
        <div class='page-header'></div>
        <div class='panel panel-default'>
            <div class='panel-body'>
                <div id='output' class='list-group' style='overflow:auto;height:600px'>
                </div>
            </div>
            <div class='panel-footer'>
                <div class='input-group'>
                    <span class='input-group-addon' id='basic-addon1'>COMMAND</span>
                    <input id='input' type='text' class='form-control' placeholder='Press ENTER to execute' aria-label='Amount(to the nearest dollar)'>
                </div>
            </div>
        </div>
    </div>

    <script>
    $('#input').keydown(function (e)
    {
        if (e.keyCode == 13) { // Enter
            // we don't want a line break in the console
            e.preventDefault();
            runCommand($('#input').val());
        } else if (e.keyCode == 38) { // Up
            previousCommand();
        } else if (e.keyCode == 40) { // Down
            nextCommand();
        } else if (e.keyCode == 27) { // Escape
            resetInput();
        } else if (e.keyCode == 9) { // Tab
            e.preventDefault();
            complete($('#input').val());
        }
    });
    </script>
  </body>
</html>

        ";
    }
}