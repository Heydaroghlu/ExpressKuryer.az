

$(function () {

    $(document).on("click", ".btn-delete-sweet", function (e) {

        e.preventDefault();

        let url = $(this).attr("href");
        console.log(url);

        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(url)
                    .then(response => response.text())
                    .then(data => {
                        console.log(data);
                        location.reload();
                    })
            }
        })
    })
})