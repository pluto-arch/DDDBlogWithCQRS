#计算在线时长存储过程
##################################
use zylblog;
DROP PROCEDURE IF EXISTS `login_time_length`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `login_time_length`(IN end_time datetime,IN uid int, OUT sum int)
BEGIN
    set @start_time = (SELECT Logged FROM zylblog.blog_log where Message like '%用户登录%' and userid=uid order by Logged desc limit 1);
    set sum  = (SELECT TIMESTAMPDIFF(SECOND ,@start_time,end_time));
    select sum;
END
;;
DELIMITER ;
##################################
