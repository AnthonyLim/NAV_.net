/* start of addition January 09, 2012 */
/*createLinkHandler = function(element, popupItemID) {
element.onclick = "getFlickerSolved(" + popupItemID + ")";
}*/


PopupValidation = function(txtBoxID, PopupID) {
    if (document.getElementById(txtBoxID).value != '')
    { getFlickerSolved(PopupID); }
}
getFlickerSolved = function(ClientID) {
    document.getElementById(ClientID).style.display = 'none';
}
/* end of addition January 09, 2012 */