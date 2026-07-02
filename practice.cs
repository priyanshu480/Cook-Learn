<!DOCTYPE html>
<html>
<head>
    <title>array operations</title>
   
</head>
<body>
    <h1>array operations</h1>
    //write your code here
    <div>
    <label>Enter Number 1<spna style="color: red;">*</spna></label>
    <input type="number" name="num1" id="num1">
</div>
 
<div>
    <label>Enter Number 2<spna style="color: red;">*</spna></label>
    <input type="number" name="num2" id="num2">
</div>
 
<div>
    <label>Enter Number 3<spna style="color: red;">*</spna></label>
    <input type="number" name="num3" id="num3">
</div>
 
<div>
    <label>Enter Number 4<spna style="color: red;">*</spna></label>
    <input type="number" name="num4" id="num4">
</div>
 
<div>
    <label>Enter Number 5<spna style="color: red;">*</spna></label>
    <input type="number" name="num5" id="num5">
</div>
 
<button id= "calculateButton" onclick="calculate()">Calculate</button>
 
<div id="result">
    <p id="errorMessage" style="color: red;"></p>
    </div>
 
    <div id="maximumNo">
       <span id="sp1"></span>
        </div>
 
        <div id="minimumNo">
            <span id="sp2"></span>
             </div>
 
             <div id="sumOfAllNumbers">
                <span id="sp3"></span>
                 </div>
 
                 <script src="script.js"></script>
 
</body>
</html>
 
 
 
 
function calculate() {
    const ids = ["num1", "num2", "num3", "num4", "num5"];
    const numArr = ids.map(id => Number((<HTMLInputElement>document.getElementById(id)).value));
 
    if(numArr.some(n => !n)) {
        document.getElementById("errorMessage")!.innerHTML = "Enter all the numbers";
        return;
    }
 
    const maxNum = Math.max(...numArr);
    const minMum = Math.min(...numArr);
    const sum = numArr.reduce((acc,n) => acc + n, 0);
 
    document.getElementById("sp1")!.innerHTML =`Maximum number: ${maxNum}`;
    document.getElementById("sp2")!.innerHTML =`Minimum number: ${minMum}`;
    document.getElementById("sp3")!.innerHTML =`Sum of all numbers: ${sum}`;
}
 
function calculate() {
    var ids = ["num1", "num2", "num3", "num4", "num5"];
    var numArr = ids.map(function (id) { return Number(document.getElementById(id).value); });
    if (numArr.some(function (n) { return !n; })) {
        document.getElementById("errorMessage").innerHTML = "Enter all the numbers";
        return;
    }
    var maxNum = Math.max.apply(Math, numArr);
    var minMum = Math.min.apply(Math, numArr);
    var sum = numArr.reduce(function (acc, n) { return acc + n; }, 0);
    document.getElementById("sp1").innerHTML = "Maximum number: ".concat(maxNum);
    document.getElementById("sp2").innerHTML = "Minimum number: ".concat(minMum);
    document.getElementById("sp3").innerHTML = "Sum of all numbers: ".concat(sum);
}
 
 
