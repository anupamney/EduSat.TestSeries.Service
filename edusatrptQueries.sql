-- database: c:\Users\Anupam.Shandilya\source\Work\EduSat.TestSeries.Service\app.db

-- Use the ▷ button in the top right corner to run the entire file.

SELECT 
    sch.name,
    sch.school_id,
    tea.first_name,
    tea.last_name,
    tea.email_id,
    sd.total_students,
    pd.paid,
    pd.TOTAL_PAYMENT,
    pd.payment_status
FROM 
    scholarships_details sd 
LEFT JOIN 
    schools sch ON sch.school_id = sd.school_id
LEFT JOIN 
    teachers tea ON sd.teacher_id = tea.id
LEFT JOIN 
    class cl ON cl.id = sd.class_id
LEFT JOIN 
    payment_details pd ON pd.scholarship_id = sd.id;

