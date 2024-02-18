document.querySelector("#load-friends-button").addEventListener("click",
    async function () {
        var response = await fetch("friends-list", { method: "GET" });
        var responsebody = response.text();
        document.querySelector("#list").innerHTML = responsebody;
    });