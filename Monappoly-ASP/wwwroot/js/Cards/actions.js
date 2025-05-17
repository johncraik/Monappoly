let baseUrl = '/../../CardAction/';
function CreateAction(groupId){
    document.getElementById('group-id').value = groupId;
    let frm = document.getElementById('form');
    frm.submit();
}

function GetActionPartial(list, action){
    let i = list.options.selectedIndex;
    let selected = list.options[i].value;
    
    let url = baseUrl + 'Get' + action + 'ActionPartial?type=' + selected;
    let target = document.getElementById('move-action');
    FetchPartial(url, target);
}