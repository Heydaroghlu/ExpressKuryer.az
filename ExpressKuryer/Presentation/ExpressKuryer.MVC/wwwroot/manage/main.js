
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
                        location.reload();
                    })
            }
        })
    })
})

let input = document.querySelectorAll(".img-upload-input")
let hoverInput = document.querySelector(".hover-image-input")
let posterInput = document.querySelector(".poster-image-input")
let imgBox = document.querySelectorAll(".img-box");
let imgBoxs = document.querySelectorAll(".img-boxs");
let imgBoxBig = document.querySelectorAll(".img-box-big");
let hoverImg = document.querySelector(".hover-image-change img")
let posterImg = document.querySelector(".poster-image-change img")
let prodAllImgs = document.querySelector(".product-all-images")

let imgBoxItem = document.querySelector(".img-box-item");
let imgBoxItems = document.querySelectorAll(".img-box-item");
var imgBoxClone;


input.forEach(function (elem) {

    elem.onchange = function (e) {

        let files = e.target.files;
        let filesArr = Array.from(files);

        filesArr.forEach(x => {

            if (x.type.startsWith("image/")) {
                let reader = new FileReader()

                reader.onload = function () {

                    imgBox.forEach(function (e) {
                        e.innerText = "";
                        let newImg = document.createElement("img");

                        newImg.setAttribute("src", reader.result)
                        newImg.style.width = "250px"
                        newImg.style.height = "250px"
                        newImg.style.objectFit = "cover"

                        e.appendChild(newImg);  
                    })

                    imgBoxBig.forEach(function (e) {
                        let newImg = document.createElement("img");

                        newImg.setAttribute("src", reader.result)
                        newImg.style.width = "400px"    
                        newImg.style.height = "400px"
                        newImg.style.objectFit = "cover"

                        e.appendChild(newImg);
                        let prev = newImg.previousElementSibling;
                        prev.classList.add("d-none");
                    })

                    //imgBoxItems.forEach(function (e) {
                    //    imgBoxClone = e;
                    //    console.log("asdasdasd");
                    //    console.log(e);
                    //    e.remove();
                    //})

                    imgBoxs.forEach(function (e) {
                        var newElement = imgBoxClone.cloneNode(true);
                        newElement.firstElementChild.setAttribute("src", reader.result)
                        e.appendChild(newElement);
                    })
                }
                reader.readAsDataURL(x);
            }
        })
    }
})

posterInput.onchange = function (e) {

    let files = e.target.files;
    let filesArr = Array.from(files);

    filesArr.forEach(x => {
        if (x.type.startsWith("image/")) {
            let reader = new FileReader()
            reader.onload = function () {
                posterImg.style.display = "block";
                posterImg.parentElement.lastElementChild.style.display = "block";
                posterImg.setAttribute("src", reader.result)
            }
            reader.readAsDataURL(x);
        }
    })
}

hoverInput.onchange = function (e) {
    let files = e.target.files;
    let filesArr = Array.from(files);

    filesArr.forEach(x => {
        if (x.type.startsWith("image/")) {
            let reader = new FileReader()
            reader.onload = function () {
                hoverImg.setAttribute("src", reader.result)
            }
            reader.readAsDataURL(x);
        }
    })
}


let productinput = document.getElementById("product-input")
let productdiv = document.getElementById("product-div")
productinput.onchange = function (e) {
    let files = e.target.files
    let filesarr = [...files]
    filesarr.forEach(x => {
        if (x.type.startsWith("image/")) {
            let reader = new FileReader()
            reader.onload = function () {
                let newimg = document.createElement("img")
                newimg.style.width = "190px"
                newimg.setAttribute("src", reader.result)
                productdiv.appendChild(newimg)
            }
            reader.readAsDataURL(x)
        }
    })
}

$(document).ready(function () {
    $(document).on("click", ".remove-img-box", function () {
        $(this).parent().remove();
    })
})


