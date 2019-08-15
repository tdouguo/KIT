#!/bin/bash  
executCommand=$1  
begin_time=$(date "+%s")  
echo -e "begin_time = $begin_time"  
bash $executCommand
end_time=$(date "+%s")  
echo -e "end_time = $end_time"  
  
time_distance=$(($end_time - $begin_time));  
hour_distance=$(expr ${time_distance} / 3600)    
hour_remainder=$(expr ${time_distance} % 3600)    
min_distance=$(expr ${hour_remainder} / 60)    
min_remainder=$(expr ${hour_remainder} % 60)   
echo -e "spend time is ${hour_distance}小时${min_distance}分${min_remainder}秒"
