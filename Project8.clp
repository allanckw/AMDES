;;Every time a patient is selected, load information in to KB
;;Or Create a new patient.
;;Data is loaded from a XML File (For Past cases) 
;to reduce Platform dependency on DB -> to UI -> CLIPS
(deftemplate Patient
   (slot title (type STRING))
   (slot SURNAME (type STRING))
   (slot FIRSTNAME (type STRING))
   (slot AGE (type STRING)) ;important parameter 
)

;To be confirmed
;(deftemplate Question )

;;for future use 
(deftemplate Test-Results
	(slot TestName (type STRING))
	(slot DateOrdered (type String))
	(slot Result (type String) (default "Normal"))
)

;(deftemplate Recommendations



