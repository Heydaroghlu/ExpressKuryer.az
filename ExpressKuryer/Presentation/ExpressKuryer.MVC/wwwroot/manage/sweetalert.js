

$(function () {

    $(document).on("click", ".btn-delete-sweet", function (e) {

        e.preventDefault();

        let url = $(this).attr("href");
        console.log(url);

        Swal.fire({
            title: 'Əminsiniz?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText: 'Xeyr',
            confirmButtonText: 'Bəli, sil'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(url)
                    .then(response => response.text())
                    .then(data => {
                        console.log(data);


                        const success = document.createElement("input");
                        success.id = "toaster-success";
                        success.innerHTML = "adfafd";
                        success.value = "Silindi";
                        document.querySelector("body").appendChild(success);

                        if ($("#toaster-success").length) {
                            toastr["success"]($("#toaster-success").val())
                        }


                        location.reload();
                    })
            }
        })
    })
})