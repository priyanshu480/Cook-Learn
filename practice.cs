<!DOCTYPE html>
<html>
<head>
    <title>array operations</title>
   
</head>
<body>
   <h1>array operations</h1>
   <div>
    <label>Enter Number 1 <spna style="color: red;">*</spna></label>
    <input type="number" name ="num1" id = "num1">
   </div>
 
   <div>
    <label>Enter Number 2 <spna style="color: red;">*</spna></label>
    <input type="number" name ="num2" id = "num2">
   </div>
 
   <div>
    <label>Enter Number 3 <spna style="color: red;">*</spna></label>
    <input type="number" name ="num3" id = "num3">
   </div>
 
   <div>
    <label>Enter Number 4 <spna style="color: red;">*</spna></label>
    <input type="number" name ="num4" id = "num4">
   </div>
 
   <div>
    <label>Enter Number 5 <spna style="color: red;">*</spna></label>
    <input type="number" name ="num5" id = "num5">
   </div>
 
   <button id="calculateButton" onclick="calculate()">Calculate</button>
 
   <div id="result">
    <p id="errorMessage" style="color: red;"></p>
   </div>
 
   <div id="sumOfEven">
   <span id="sp1"></span>
   </div>
 
   <div id = "numbersGreaterThan5">
    <span id="sp2"></span>
   </div>
 
   <script src="script.js"></script>
 
</body>
</html>
 
 
 
 
function calculate()
{
    const ids = ["num1" , "num2", "num3", "num4", "num5"];
 
    const numArr = ids.map(id => Number((<HTMLInputElement>document.getElementById(id)).value));
 
    if(numArr.some( n=> !n)){
        document.getElementById("errorMessage")!.innerHTML = "Enter all the numbers";
        return;
    }
   
    // const maxNum = Math.max(...numArr);
    // const minNum = Math.min(...numArr);
    const nums = numArr.filter( n => n >= 5);
    const sum = numArr.reduce((acc, n) => acc + (n % 2 === 0 ? n : 0), 0);
 
    document.getElementById("sp1").innerHTML = `Sum of even numbers: ${sum}`;
    document.getElementById("sp2").innerHTML = `Numbers greater than 5: ${nums.join(",")}`;
}
 
function calculate() {
    var ids = ["num1", "num2", "num3", "num4", "num5"];
    var numArr = ids.map(function (id) { return Number(document.getElementById(id).value); });
    if (numArr.some(function (n) { return !n; })) {
        document.getElementById("errorMessage").innerHTML = "Enter all the numbers";
        return;
    }
    // const maxNum = Math.max(...numArr);
    // const minNum = Math.min(...numArr);
    var nums = numArr.filter(function (n) { return n >= 5; });
    var sum = numArr.reduce(function (acc, n) { return acc + (n % 2 === 0 ? n : 0); }, 0);
    document.getElementById("sp1").innerHTML = "Sum of even numbers: ".concat(sum);
    document.getElementById("sp2").innerHTML = "Numbers greater than 5: ".concat(nums.join(","));
}
 
 
