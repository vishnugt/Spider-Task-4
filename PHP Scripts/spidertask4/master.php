<?php
$variable = $_GET['data'];
$handle = fopen('data.php','a');
fwrite($handle, $variable);
fwrite($handle,'*');
fclose($handle);

$handle2 = file('mastercounter.php');
foreach($handle2 as $counter){
$handle3 = fopen('mastercounter.php','w');
fwrite($handle3,++$counter);
fclose($handle3);
}



echo "Successful";
?>