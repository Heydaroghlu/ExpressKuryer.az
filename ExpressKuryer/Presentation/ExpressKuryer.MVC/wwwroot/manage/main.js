
var inputFiles = document.querySelectorAll(".file");
var multiInputFiles = document.querySelectorAll(".multi-file");

inputFiles.forEach(element => {
    var imageBox = document.querySelector(".image-box");

    
    element.onchange = function (e) {
        let files = e.target.files
        let filesarr = [...files]
        var images = document.querySelectorAll(".image-box img");

        filesarr.forEach(x => {


            if (x.type.startsWith("image/")) {
                let reader = new FileReader()
                reader.onload = function () {
                    let newimg = document.createElement("img")
                    newimg.style.width = "300px";
                    newimg.style.margin = "10px 0px";
                    newimg.setAttribute("src", reader.result)

                    images.forEach(e=>e.remove())

                    imageBox.appendChild(newimg)
                }
                reader.readAsDataURL(x)
            }
        })
    }
});


multiInputFiles.forEach(element => {

    var imageBox = document.querySelector(".image-boxs");

    element.onchange = function (e) {
        let files = e.target.files
        let filesarr = [...files]
        var images = document.querySelectorAll(".image-boxs img");

        filesarr.forEach(x => {

            if (x.type.startsWith("image/")) {
                let reader = new FileReader()
                reader.onload = function () {
                    let newimg = document.createElement("img")
                    newimg.style.width = "300px";
                    newimg.style.margin = "10px";
                    newimg.setAttribute("src", reader.result)

                    images.forEach(e => e.remove())

                    imageBox.appendChild(newimg)
                    console.log("asd");
                }
                reader.readAsDataURL(x)
            }
        })
    }
});

let dropdownDeliveryItems = document.querySelectorAll(".delivery-list .delivery-btn");

dropdownDeliveryItems.forEach(x => {
    x.addEventListener("click", function (e) {
        e.preventDefault();
        let href = x.getAttribute("href");

        fetch(href).then(response => {
            response.text();
        }).then(data => {

            var element = document.querySelector("#toaster-success")
            var delivery = document.querySelector(".deliverystatus")

            if (element != null) {
                element.value = e.srcElement.getAttribute("value") + " oldu";
                console.log(e.srcElement.getAttribute("value"));

                if (e.srcElement.getAttribute("value") == "İmtina") {
                    delivery.className = "btn bgl-danger text-danger deliverystatus";
                } else if (e.srcElement.getAttribute("value") == "Qəbul") {
                    delivery.className = "btn bgl-success text-success deliverystatus";
                } else {
                    delivery.className = "btn bgl-warning text-warning deliverystatus";
                }

                delivery.innerHTML = e.srcElement.getAttribute("value");
            } else {
                const success = document.createElement("input");
                success.id = "toaster-success";
                success.innerHTML = "adfafd";
                success.value = e.srcElement.getAttribute("value") + " oldu";

                if (e.srcElement.getAttribute("value") == "İmtina") {
                    delivery.className = "btn bgl-danger text-danger deliverystatus";
                } else if (e.srcElement.getAttribute("value") == "Qəbul") {
                    delivery.className = "btn bgl-success text-success deliverystatus";
                } else {
                    delivery.className = "btn bgl-warning text-warning deliverystatus";
                }

                delivery.innerHTML = e.srcElement.getAttribute("value");
                document.querySelector("body").appendChild(success);
                console.log(e.srcElement.getAttribute("value"));
            }

            if ($("#toaster-success").length) {
                toastr["success"]($("#toaster-success").val())
            }


        })
    });
});
