<?php
if(isset($_GET["app"]) && isset($_GET["cmd"])){
	//RESTCALL
	//TODO: database!
	if($_GET["app"] == "simba"){
		if($_GET["cmd"]=="update"){
			die("1.0");		
		} else {
			die("error");
		}
	} else {
		die("error");
	}
}
?>
<html>
	<head>
		<title>WebDaD :: Toolkit</title>
	</head>
	<body>
		<h1>REST</h1>
		<p>This is still in contruction!</p>
	</body>
</html>
