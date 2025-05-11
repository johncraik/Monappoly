function ChangeTenant(){
    Swal.fire({
        title: 'Change Tenant?',
        icon: 'question',
        input: 'text',
        inputLabel: 'Input the Tenant ID',
        showDenyButton: true,
        denyButtonText: 'Cancel',
        confirmButtonText: 'Set Tenant'
    }).then((res) => {
        if(res.isConfirmed){
            let frm = document.getElementById('tenant-form');
            let inp = document.getElementById('tenant-id');
            inp.value = res.value;
            frm.submit();
        }
    })
}

function UpdateUserDialog(url, id, key, title, text){
    Swal.fire({
        title: title,
        icon: 'question',
        input: 'text',
        inputLabel: text,
        showDenyButton: true,
        denyButtonText: 'Cancel',
        confirmButtonText: 'Update'
    }).then((res) => {
        if(res.isConfirmed){
            $.ajax({
                type: 'POST',
                url: url + '?userId=' + id + '&' + key + '=' + res.value,
                success: function (res){
                    if(res === true){
                        Swal.fire("User Updated", "", "success").then((r) => {
                            location.reload();
                        });
                    }
                    else{
                        Swal.fire("Unable to update user.", "", "error");
                    }
                }
            });
        }
    })
}

function UpdateUserName(id){
    let url = '/User/ChangeUserName';
    UpdateUserDialog(url, id, 'username', 'Change Username?', 'Input the new username');
}

function UpdateDisplayName(id){
    let url = '/User/ChangeUserDisplayName';
    UpdateUserDialog(url, id, 'displayName', 'Change Display Name?', 'Input the new display name');
}

function UpdateEmail(id){
    let url = '/User/ChangeUserEmail';
    UpdateUserDialog(url, id, 'email', 'Change Email?', 'Input the new email');
}

function UpdatePhone(id){
    let url = '/User/ChangeUserPhoneNumber';
    UpdateUserDialog(url, id, 'phone', 'Change Phone Number?', 'Input the new phone number');
}