# progress-definition-to-sql

I did not like the default script generated by the DataTool,
so I wrote this simple application that can take exported progress definitions and
create a better script with better table names.

This is all about definition and schema. No data will be included.

## Good things
1. Can merge multiple definitions into one script
2. Keeps the casing for table names and field names
3. Keeps the indecis
4. Keeps the description
5. Create foreign keys based on column names and primary keys

## Bad things
1. Does not create the correct type
  Could easily be fixed but I was more intrested in exploring the database
  It won't for instance create correct lenght for string fields
2. Foreign keys will not be created if the type does not match

### Example definition blocks
ADD SEQUENCE "TheSequence"
  INITIAL 1
  INCREMENT 1
  CYCLE-ON-LIMIT no
  MIN-VAL 1

ADD TABLE "T99Table"
  AREA "tableArea"
  DESCRIPTION "Some description for the table"
  DUMP-NAME "t99table"

ADD FIELD "One" OF "T99Table" AS integer 
  FORMAT "->,>>>,>>9"
  INITIAL "0"
  POSITION 2
  MAX-WIDTH 4
  ORDER 10

ADD FIELD "Two" OF "T99Table" AS integer 
  FORMAT ">>9"
  INITIAL "0"
  POSITION 3
  MAX-WIDTH 4
  ORDER 20

ADD INDEX "M_Key" ON "T99Table" 
  AREA "tableArea"
  PRIMARY
  INDEX-FIELD "One" ASCENDING 

ADD INDEX "OtherKey" ON "T99Table" 
  AREA "tableArea"
  INDEX-FIELD "Two" DESCENDING
  INDEX-FIELD "One" ASCENDING
