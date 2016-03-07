--------------------------------------------------------
--  文件已创建 - 星期三-二月-24-2016   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Table EXCEPTIONINFO
--------------------------------------------------------

  CREATE TABLE "MIANYANG"."EXCEPTIONINFO" 
   (	"E_ID" NUMBER(38,0), 
	"B_ID" NUMBER(38,0), 
	"C_ID" NUMBER(38,0), 
	"D_ID" NUMBER(38,0), 
	"CONTENT" VARCHAR2(100 BYTE), 
	"DATATIME" VARCHAR2(25 BYTE)
   ) PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT)
  TABLESPACE "USERS" ;
REM INSERTING into MIANYANG.EXCEPTIONINFO
SET DEFINE OFF;
Insert into MIANYANG.EXCEPTIONINFO (E_ID,B_ID,C_ID,D_ID,CONTENT,DATATIME) values (1,2,2,5,'特排管道测量装置数据实时值高于阈值','22016-2-24 16-26-20');
Insert into MIANYANG.EXCEPTIONINFO (E_ID,B_ID,C_ID,D_ID,CONTENT,DATATIME) values (22,4,3,18,'数据异常','22016-2-24 16-27-30');
--------------------------------------------------------
--  DDL for Index EXCEPTIONINFO_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "MIANYANG"."EXCEPTIONINFO_PK" ON "MIANYANG"."EXCEPTIONINFO" ("E_ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT)
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  Constraints for Table EXCEPTIONINFO
--------------------------------------------------------

  ALTER TABLE "MIANYANG"."EXCEPTIONINFO" ADD CONSTRAINT "EXCEPTIONINFO_PK" PRIMARY KEY ("E_ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT)
  TABLESPACE "USERS"  ENABLE;
 
  ALTER TABLE "MIANYANG"."EXCEPTIONINFO" MODIFY ("E_ID" NOT NULL ENABLE);
