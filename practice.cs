<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport"
    content="width==device-width, initial-scale=1.0">
    <title>Library Management System</title>
    <link rel="stylesheet" href="style.css">
   
</head>
<body>
         <h1>Library Management System</h1>
 
         <div id="bookGrid">
            <div class="grid">
                <div="box">
                    <p> 1984 by George Orwell</p>
                    <p> Is Borrowed <span id="sp0"></span></p>
 
                    <div class="grid">
                        <button id="borrowButton-0" class="borrow">Borrow</button>
                        <button id="returnButton-0" class="return">Return</button>
                    </div>
                </div>
 
                <div class="box">
                    <p> To Kill some Bird</p>
                    <p> Is Borrowed <span id="sp1"></span></p>
 
                    <div class="grid">
                        <button id="borrowButton-1" class="borrow">Borrow</button>
                        <button id="returnButton-1" class="return">Return</button>
                    </div>
 
                </div>
                <div class="box">
                    <p> The Great Gatsby</p>
                    <p> Is Borrowed <span id="sp2"></span></p>
 
                    <div class="grid">
                        <button id="borrowButton-2" class="borrow">Borrow</button>
                        <button id="returnButton-2" class="return">Return</button>
                    </div>
                    </div>
 
         </div>
         </div>
 
         <script src="script.js"></script>
</body>
</html>
 
 
 
 
function setupBorrowReturn(index: number){
    const borrowBtn = document.getElementById(`borrowButton-${index}`)as HTMLInputElement;
    const returnBtn = document.getElementById(`returnButton-${index}`)as HTMLInputElement;
    const status = document.getElementById(`sp${index}`)!;
 
    borrowBtn.addEventListener("click",() => {
        status.innerHTML = "True";
        returnBtn.disabled = false;
        borrowBtn.disabled = true;
    });
 
    returnBtn.addEventListener("click",() => {
        status.innerHTML = "False";
        returnBtn.disabled = true;
        borrowBtn.disabled = false;
    });
}
 
[0,1,2].forEach(setupBorrowReturn);
 
function setupBorrowReturn(index) {
    var borrowBtn = document.getElementById("borrowButton-".concat(index));
    var returnBtn = document.getElementById("returnButton-".concat(index));
    var status = document.getElementById("sp".concat(index));
    borrowBtn.addEventListener("click", function () {
        status.innerHTML = "True";
        returnBtn.disabled = false;
        borrowBtn.disabled = true;
    });
    returnBtn.addEventListener("click", function () {
        status.innerHTML = "False";
        returnBtn.disabled = true;
        borrowBtn.disabled = false;
    });
}
[0, 1, 2].forEach(setupBorrowReturn);
 
 
