window.addEventListener('load', function () {
    const fetchInterval = 5000;
    setInterval(FetchStockPrice, fetchInterval);
});

async function FetchStockPrice() {
    const stockSymbol = document.getElementById('StockSymbol').value;
    await fetch("api/v1/StockPriceQuote?stockSymbol=" + stockSymbol, { method: "GET" })
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