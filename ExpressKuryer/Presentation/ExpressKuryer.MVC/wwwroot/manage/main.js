
var inputFiles = document.querySelectorAll(".file");

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