let baseUrl = '/../../Board/';
function GetBoards(id){
    let board = document.getElementById('board-id');
    board.innerText = id;
    
    let boardNavs = document.getElementsByClassName('board-nav');
    for (let i = 0; i < boardNavs.length; i++){
        boardNavs[i].classList.remove('active');
    }
    let boardNav = document.getElementById('board-' + id);
    boardNav.classList.add('active');
    
    let url = baseUrl + 'GetBoardViewPartial?boardId=' + id;
    let target = document.getElementById('board-view');
    FetchPartial(url, target);
}

function DeleteBoard(id){
    Swal.fire({
        title: "Delete Board?",
        icon: "question",
        showDenyButton: true,
        confirmButtonText: "Yes",
        denyButtonText: "Cancel"
    }).then((result) => {
        if(result.isConfirmed){
            $.ajax({
                type: 'POST',
                url: baseUrl + 'DeleteBoard?id=' + id,
                success: function (res){
                    if(res === true){
                        Swal.fire("Board Deleted", "", "success").then((r) => {
                            location.reload();
                        });
                    }
                    else{
                        Swal.fire("Unable to Delete Board", "", "error");
                    }
                }
            })
        }
    })
}