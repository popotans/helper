<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>#title#-#sitename#</title>
</head>
<body>
and now iKnow who are you :<h3>#person.Name#,#person.Age#,#person.Salary#,#person.HasChild#,##,##</h3>
and below is your all Students：

    <table>
<ad:foreach collection="#list#" var="p" index="i">
   <tr>
        <td>#p.name#</td>
        <td>#p.Company#</td>
        <td>#p.haschild#</td>
        <td>#p.Salay#</td>
        <td>#p.School#</td>
        <td>#iif(lt(p.age,25),"<span style='color:red'>","<span style='color:blue'>")# #p.AgE#</span>
        
        </td>
   </tr>
</ad:foreach>
    </table>
</body>
</html>
