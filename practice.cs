function showDetails()
{
    let name = (<HTMLInputElement>document.getElementById("nameInput")).value;
    let age = (<HTMLInputElement>document.getElementById("ageInput")).value;
    let hobbies = (<HTMLInputElement>document.getElementById("arrayInput")).value.split(",");
    let isStudent = (<HTMLInputElement>document.getElementById("isStudentSelect")).value;
 
    let TrueFasle = isStudent == "True"? true:false;
 
    document.getElementById("out1").innerHTML = "Name: "+name;
    document.getElementById("out11").innerHTML = "Type: "+typeof(name);
 
    document.getElementById("out2").innerHTML = "Age: "+age;
    document.getElementById("out22").innerHTML = "Type: "+typeof(age);
 
    document.getElementById("out3").innerHTML = "Hobbies: "+hobbies;
    document.getElementById("out33").innerHTML = "Type: "+typeof(hobbies);
 
    document.getElementById("out4").innerHTML = "Student: "+isStudent;
    document.getElementById("out44").innerHTML = "Type: "+typeof(TrueFasle);
   
 
}
 
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>User Information</title>
</head>
<body>
    <div>
        <h2>User Information</h2>
        <label for ="nameInput">Enter name: </label>
        <input id ="nameInput" >
 
        <label for ="ageInput">Enter age: </label>
        <input id ="ageInput">
 
        <label for ="arrayInput">Enter hobbies(comma-separated): </label>
        <input id ="arrayInput">
       
        <label for="isStudentSelect">Are you a student?(Select true/false): </label>
        <select id="isStudentSelect">
            <option value="true">True</option>
            <option value="false">False</option>
        </select>
       
    </div>
 
    <button id="showInfoButton" onclick="showDetails()">Show Information</button>
 
    <div id="output">
        <span id="out1"></span><span id="out11"></span><br>
        <span id="out2"></span><span id="out22"></span><br>
        <span id="out3"></span><span id="out33"></span><br>
        <span id="out4"></span><span id="out44"></span><br>
    </div>
   
 
 
 
 
   
    <script src="script.js"></script>
</body>
</html>




         function showDetails() {
    var name = document.getElementById("nameInput").value;
    var age = document.getElementById("ageInput").value;
    var hobbies = document.getElementById("arrayInput").value.split(",");
    var isStudent = document.getElementById("isStudentSelect").value;
    var TrueFasle = isStudent == "True" ? true : false;
    document.getElementById("out1").innerHTML = "Name: " + name;
    document.getElementById("out11").innerHTML = "Type: " + typeof (name);
    document.getElementById("out2").innerHTML = "Age: " + age;
    document.getElementById("out22").innerHTML = "Type: " + typeof (age);
    document.getElementById("out3").innerHTML = "Hobbies: " + hobbies;
    document.getElementById("out33").innerHTML = "Type: " + typeof (hobbies);
    document.getElementById("out4").innerHTML = "Student: " + isStudent;
    document.getElementById("out44").innerHTML = "Type: " + typeof (TrueFasle);
}
 
