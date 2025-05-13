let baseUrl = '/../../Card/';
function SetupAction(id){
    location.assign('/Objects/Cards/Action/' + id);
}

function NewCard(){
    let deckId = document.getElementById('deck-id').innerText;
    location.assign('/Objects/Cards/Edit/' + deckId + '/0');
}

function CopyCards(){
    let deckId = document.getElementById('deck-id').innerText;
    location.assign('/Objects/Cards/Copy/' + deckId);
}

function DeleteCard(id){
    let deckId = document.getElementById('deck-id').innerText;
    Swal.fire({
        title: "Delete Card?",
        icon: "question",
        showDenyButton: true,
        confirmButtonText: "Yes",
        denyButtonText: "Cancel"
    }).then((result) => {
        if(result.isConfirmed){
            $.ajax({
                type: 'POST',
                url: baseUrl + 'DeleteCard?id=' + id,
                success: function (res){
                    if(res === true){
                        Swal.fire("Card Deleted", "", "success").then((r) => {
                            GetCards(deckId);
                        });
                    }
                    else{
                        Swal.fire("Unable to Delete Card", "", "error");
                    }
                }
            })
        }
    })
}

function DeleteCards(){
    let deckId = document.getElementById('deck-id').innerText;
    Swal.fire({
        title: "Delete All Cards?",
        icon: "question",
        showDenyButton: true,
        confirmButtonText: "Yes",
        denyButtonText: "Cancel"
    }).then((result) => {
        if(result.isConfirmed){
            $.ajax({
                type: 'POST',
                url: baseUrl + 'DeleteCards?deckId=' + deckId,
                success: function (res){
                    if(res === true){
                        Swal.fire("Cards Deleted", "", "success").then((r) => {
                            GetCards(deckId);
                        });
                    }
                    else{
                        Swal.fire("Unable to Delete Cards", "", "error");
                    }
                }
            })
        }
    })
}

function GetCards(deckId){
    let deck = document.getElementById('deck-id');
    deck.innerText = deckId;
    
    let deckNavs = document.getElementsByClassName('deck-nav');
    for (let i = 0; i < deckNavs.length; i++) {
        deckNavs[i].classList.remove('active');
    }
    let deckNav = document.getElementById('deck-' + deckId);
    deckNav.classList.add('active');
    
    let url = baseUrl + 'GetCardsTable?deck=' + deckId;
    let table = document.getElementById('cards-table');
    FetchPartial(url, table);
}