window.addEventListener('load', function () {
    const fetchInterval = 5000;
    setInterval(FetchStockPrice, fetchInterval);
});

async function FetchStockPrice() {
    await fetch("api/v1/StockPriceQuote?stockSymbol=@Model.StockSymbol", { method: "GET" })
        .then(function (response) {
            return response.json();
        })
        .then(function (data) {
            UpdateStockPrice(data);
        });
}
// Update: Prevents duplicate rendering
function UpdateStockPrice(data) {
    const stockPrice = document.getElementById('price');
    stockPrice.innerHTML = data.c;
}