$(document).ready(function () {
    $("#btnLogin").click(function () {
        var userName = $("#txtUserName").val();
        var password = $("#txtPassword").val();

        var parData = {
            "Username": userName,
            "Password": password
        };
        console.log(JSON.stringify(parData));

        //$.ajax({
        //    type: "POST",
        //    url: "https://localhost:44333/api/authenticate/login",
        //    data: JSON.stringify({
        //        Username: userName,
        //        Password: password
        //    }),
        //    headers: {
        //        'Accept': 'application/json',
        //        'Content-Type': 'application/json'
        //    },
        //    success: function (data) {
        //        if (data.status == 'OK')
        //            alert('Person has been added');
        //        else
        //            alert('Failed adding person: ' + data.status + ', ' + data.errorMessage);
        //     }
        //});


        //$.post("/Home/Login",
        //    {
        //        userName: userName,
        //        password: password,
        //    }, function (result) {
        //        if (result != "Error") {
        //            localStorage.setItem("token", result);
        //            location.href = "/Home/Index";
        //        }
        //        else {
        //            alert("Yanlış Username veya Password Girdiniz.");
        //        }
        //});


        $.ajax({
            url: "https://localhost:44333/api/authenticate/login",
            //url: "/Home/Login",
            type: "POST",
            crossDomain: true,
            data: JSON.stringify(parData),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                console.log(result);
                if (result != "Error") {
                    localStorage.setItem("token", result);
                    //location.href = "/Home/Index";
                }
                else {
                    alert("Yanlış Username veya Password Girdiniz.");
                }

                //console.log( result);
                //if (result.message =="OK") {
                //    localStorage.setItem('Token', JSON.stringify(result));
                //    location.href = "/Home/Index";
                //}
                //else {
                //    alert("Hatalı kullanıcı adı veya parola");
                //}
            }
            ,
            error: function (xhr, status, error) {
                console.log(status + " sds " + error);
            }
        });

        $("#btnLogin").focus();
    });
});