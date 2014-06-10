TRUNCATE TABLE tSysInfo
go
INSERT INTO tSysInfo(SECTION,OptionName,OptionValue)
SELECT 'SYS','ProjectName','移动查询平台'
UNION ALL
SELECT 'SYS','CompanyName','xxxx有限公司'
UNION ALL
SELECT 'SYS','AuthCode',''''