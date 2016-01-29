
function BlockUIElement(elem,type)
{   
    if (elem.indexOf('#') < 0)
    {
        elem = '#' + elem;
    }    
    $(elem).append('<div style="position: absolute;top:0;left:0;width: 100%;height:100%;z-index:2;opacity:0.3;background:white;" id="ui-blocker"></div>');
    if (type == "square")
    {
        $(elem).append('<div id="blockerLoader" class="loader red" style="position:absolute;top:40%;left:50%;z-index:2;"> <div class="loader-inner ball-spin-fade-loader"><div></div><div></div><div> </div><div> </div><div> </div><div> </div><div> </div><div> </div></div></div>');
    }
    else {
        $(elem).append('<div id="blockerLoader" class="loader red" style="position:absolute;top:40%;left:50%;z-index:2;"> <div class="loader-inner ball-spin-fade-loader"><div></div><div></div><div> </div><div> </div><div> </div><div> </div><div> </div><div> </div></div></div>');
    }
}

function UnblockUIElement(elem)
{
    if (elem.indexOf('#') < 0) {
        elem = '#' + elem;
    }

    $(elem).find("#ui-blocker").remove();
    $(elem).find("#blockerLoader").remove();
}