<?php

$servername = "localhost";
$username = "root";
$password = "";
$dbname = "playerstats";

// variables submited by user
$loginUser = $_POST["loginUser"];
$loginPass = $_POST["loginPass"];

// Ceateconnection
$conn = new mysqli($servername, $username, $password, $dbname);

// check connection
if ($conn->connect_error) {
	die("Connection failed: ". $conn->connect_error);
}

$sql = "SELECT password FROM users WHERE username = '" . $loginUser . "'";

$result = $conn->query($sql);

if ($result->num_rows > 0) {
	// outpub data of each row
	while ($row = $result->fetch_assoc()){
		if ($row["password"] == $loginPass){
			echo "Login Success.";
			// Get user's data here.
			
			// Get player info.
			
			// Get Inventory.
			
			// Modify player data.
			
			// Update Inventory
			
		} else {
			echo "Wrong Credentials.";
		}
	}
} else {
	echo "Username does not exists";
}
$conn->close();

?>