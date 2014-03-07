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
	(slot DateOrdered (type STRING))
	(slot Result (type SYMBOL) (allowed-symbols Normal Abnormal) (default NIL) )
)

; ; Final Diagnosis ;location to store final diagnosis
(deftemplate diagnosis
(slot dementia (type SYMBOL) (allowed-symbols Likely Unlikely))
(slot cognitive-deficits (type SYMBOL))
(slot cognitive-impairment (type SYMBOL))
(slot evaluate-further (type SYMBOL))
(slot referral (type SYMBOL))
(slot referral-to (type SYMBOL))

)

; ; Sign of a symptom, to be asserted into KB, location to store answer 
(deftemplate symptom
(slot sign (type SYMBOL)) ;e.g. Amnesia,  Aphasia, etc
(slot Question (type SYMBOL) (default NIL) ;reason = Remembering things/event that happened recently?
(slot Answer (type SYMBOL) (allowed-symbols Yes No Unknown)) ;ans to the reason  
)

(defrule unlikely ;base case, point A -> No, the rest dies
	(symptom (sign Amnesia) (Answer No))
	=>
	(assert (diagnosis (dementia Unlikely) ))
	(printout t "Patient has a LOW likelihood of dementia")
)






