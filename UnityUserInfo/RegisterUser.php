<?php

$servername = "localhost";
$username = "root";
$password = "";
$dbname = "userinfo";

// variables submitted by user
$loginName = $_POST["loginName"];
$loginId = $_POST["loginId"];
$loginPass = $_POST["loginPass"];

// Ceateconnection
$conn = new mysqli($servername, $username, $password, $dbname);

// check connection
if ($conn->connect_error) {
	die("Connection failed: ". $conn->connect_error);
}

$sql = "SELECT userid FROM logininfo WHERE userid = '" . $loginId . "'";

$result = $conn->query($sql);

if ($result->num_rows > 0) {
	// tell user that that name is already taken
	// outpub data of each row
		echo "Username is alreay taken";
} else {
	echo "Creating user...";
	// Insert the user and password into the database
	$sql2 = "INSERT INTO logininfo (username, userid, password) VALUES ('" . $loginName . "', '" . $loginId . "','" . $loginPass . "')";
	if ($conn->query($sql2) == TRUE) {
		echo "New record created successfully";
	} else {
		echo "Error: " . $sql . "<br>" . $conn->error;
	}
}
$conn->close();

?>