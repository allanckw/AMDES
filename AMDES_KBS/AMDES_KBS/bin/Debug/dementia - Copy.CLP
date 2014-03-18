; ; Diagnosis Program adapted from Textbook p.97
; ; A0077307A ang nickolas

; ; QuestionQuestionQuestion
(deftemplate mode2criteria
(slot ID (type SYMBOL))
(slot GroupID (type SYMBOL))
(slot Answer (type SYMBOL))
)

(deftemplate mode2criteriaAttribute
(slot GroupID (type SYMBOL))
(slot AttributeName (type SYMBOL) (default NIL))
(slot AttributeValue (type INTEGER) (default 0))
(slot AttributeCompareType (type SYMBOL) (default NIL))
(slot Answer (type SYMBOL))
)

(deftemplate mode2criteriaGroup
(slot GroupID (type SYMBOL))
)




; ; Sign of a symptom
(deftemplate sign
(slot symptom (type SYMBOL))
(slot organ (type SYMBOL) (default NIL))
(slot site (type SYMBOL) (default NIL)))

; ; Final Diagnosis
(deftemplate diagnosis
(slot Conclusion (type SYMBOL))
(slot Comment (type STRING)))

; ; Sign of a symptom
(deftemplate NaviHistory
(multislot ID (type SYMBOL) (default NIL)))

; ; QuestionQuestionQuestion
(deftemplate question
(slot Id (type SYMBOL))
(slot QuestionText (type STRING))
(slot answer (type SYMBOL) (default NIL))
(slot GroupId (type SYMBOL))
;;(slot Flag (type SYMBOL) (default No))
)

(deftemplate group
(slot GroupId (type SYMBOL))
(slot SuccessType (type SYMBOL))
(slot SuccessArg (type INTEGER))
(slot TrueCount (type INTEGER) (default 0))
(slot conclusion (type SYMBOL) (default NIL))
(slot answer (type SYMBOL) (default No))
)
;;conclusion Yes/No

;;maybe dont need depends
(deftemplate Currentquestion
(slot Id (type SYMBOL))
(slot QuestionText (type STRING))
(slot answer (type STRING) (default ""))
(slot GroupId (type SYMBOL))
)

(deftemplate Currentgroup
(slot GroupId (type SYMBOL))
(slot SuccessType (type SYMBOL))
(slot SuccessArg (type INTEGER))
(slot TrueCount (type INTEGER) (default 0))
(slot conclusion (type SYMBOL) (default NIL))
(slot answer (type SYMBOL) (default No))
)

; ; navigation
(deftemplate Navigation
(slot CriteriaGroupID (type SYMBOL) (default NIL))
(slot CriteriaAnswer (type SYMBOL) (default No))
(slot Comment (type STRING) (default ""))
(slot Conclusion (type SYMBOL) (default NIL))
(slot DestinationGroupID (type SYMBOL) (default NIL))
(slot AttributeName (type SYMBOL) (default NIL))
(slot AttributeValue (type INTEGER) (default 0))
(slot AttributeCompareType (type SYMBOL) (default NIL))
)
;;if destinationgroupid == nil then means goal reached
;;age>?




; ; The initial facts
; ; i.e. Patient reports abdominal pain
(deffacts the-facts

(NaviHistory)

)

;;A->B->END

(defrule Pass-OR
?CUR_Group <- (group (GroupId ?z&~NIL) (SuccessType OR) (answer ~Yes))
(exists
	
	(question (Id ?a) (QuestionText ?b) (answer Yes) (GroupId ?z))
)
=>
;;(printout t crlf "Satisfied "?z"!" crlf)
(modify ?CUR_Group (answer Yes))
)

(defrule Pass-COUNT-no-to-yes
?CUR_Group <- (group (GroupId ?z&~NIL) (SuccessType COUNT) (TrueCount ?b) (SuccessArg ?c) (answer ~Yes))
(test(>= ?b ?c))
=>
(modify ?CUR_Group (answer Yes))
)

(defrule Pass-COUNT-yes-to-no
?CUR_Group <- (group (GroupId ?z&~NIL) (SuccessType COUNT) (TrueCount ?b) (SuccessArg ?c) (answer ~No))
(test(< ?b ?c))
=>
(modify ?CUR_Group (answer no))
)

(defrule Change-Answer
?f1 <- (choice ?id ?x)
?f2 <- (question (Id ?id) (answer ?y) (GroupId ?z))
?f3 <- (group (GroupId ?z) (TrueCount ?count))
=>
(switch ?y
(case NIL then 
	;;=>t +1
	;;=>f +-
	(if(eq Yes ?x)
	then;;yes
		(modify ?f3 (TrueCount (+ ?count 1)))
	else;;no
	)
)
(case Yes then 
	;;=>t +1
	;;=>f +-
	(if(eq Yes ?x)
	then;;yes
		
	else;;no
		(modify ?f3 (TrueCount (- ?count 1)))
	)
)
(case No then 
	;;=>t +1
	;;=>f +-
	(if(eq Yes ?x)
	then;;yes
		(modify ?f3 (TrueCount (+ ?count 1)))
	else;;no
	)
)


)
(retract ?f1)
(modify ?f2 (answer ?x))
)





(defrule init_question
(not (exists (Currentgroup)))
(Navigation (CriteriaGroupID NIL) (DestinationGroupID ?z&~NIL))
(mode 1)
?grp<-
(group 
(GroupId ?z)
(SuccessType ?a)
(SuccessArg ?b)
(TrueCount ?c)
(conclusion ?d)
(answer ?e)
)
?hist<-(NaviHistory (ID $?x))
=>
(modify ?hist (ID $?x ?z))
(printout t crlf "init" crlf)
(assert 
(Currentgroup 
(GroupId ?z)
(SuccessType ?a)
(SuccessArg ?b)
(TrueCount ?c)
(conclusion ?d)
(answer ?e)
)
)
)


;;(slot AttributeName (type SYMBOL) (default NIL))
;;(slot AttributeValue (type INTEGER) (default 0))
;;(slot AttributeCompareType (type SYMBOL) (default NIL))

;; change page here
(defrule next_question
?rmv<-(Currentgroup (GroupId ?y))
?nxt<-(next)
(group (GroupId ?y) (answer ?ans))
(Navigation (CriteriaGroupID ?y&~NIL)(CriteriaAnswer ?ans)(DestinationGroupID ?z&~NIL) (AttributeName ?an) (AttributeValue ?av) (AttributeCompareType ?at))
?att<- (attribute ?an ?attvalue)
?hist<-(NaviHistory (ID $?x))
?grp<-
(group 
(GroupId ?z)
(SuccessType ?a)
(SuccessArg ?b)
(TrueCount ?c)
(conclusion ?d)
(answer ?e)

)
=>
(printout t crlf " START next" crlf)

(if(eq NIL ?an)
then;;yes
	(retract ?rmv)
	(retract ?nxt)
	(printout t crlf "next" crlf)
	(assert 
	(Currentgroup 
	(GroupId ?z)
	(SuccessType ?a)
	(SuccessArg ?b)
	(TrueCount ?c)
	(conclusion ?d)
	(answer ?e)
	))
	(modify ?hist (ID $?x ?z))
else;;no

	(switch ?at
	(case = then 
	
		(if(= ?attvalue ?av)
		then;;yes
		(retract ?rmv)
	(retract ?nxt)
	(printout t crlf "next" crlf)
	(assert 
	(Currentgroup 
	(GroupId ?z)
	(SuccessType ?a)
	(SuccessArg ?b)
	(TrueCount ?c)
	(conclusion ?d)
	(answer ?e)
	))
	(modify ?hist (ID $?x ?z))
		else;;no
		
		)
	)

	(case <= then 
	
		(if(<= ?attvalue ?av)
		then;;yes
		(retract ?rmv)
	(retract ?nxt)
	(printout t crlf "next" crlf)
	(assert 
	(Currentgroup 
	(GroupId ?z)
	(SuccessType ?a)
	(SuccessArg ?b)
	(TrueCount ?c)
	(conclusion ?d)
	(answer ?e)
	))
	(modify ?hist (ID $?x ?z))
		else;;no
		
		)
	)
(case >= then 
	
		(if(>= ?attvalue ?av)
		then;;yes
		(retract ?rmv)
	(retract ?nxt)
	(printout t crlf "next" crlf)
	(assert 
	(Currentgroup 
	(GroupId ?z)
	(SuccessType ?a)
	(SuccessArg ?b)
	(TrueCount ?c)
	(conclusion ?d)
	(answer ?e)
	))
	(modify ?hist (ID $?x ?z))
		else;;no
		
		)
	)
	(case > then 
	
		(if(> ?attvalue ?av)
		then;;yes
		(retract ?rmv)
	(retract ?nxt)
	(printout t crlf "next" crlf)
	(assert 
	(Currentgroup 
	(GroupId ?z)
	(SuccessType ?a)
	(SuccessArg ?b)
	(TrueCount ?c)
	(conclusion ?d)
	(answer ?e)
	))
	(modify ?hist (ID $?x ?z))
		else;;no
		
		)
	)

	(case < then 
	
		(if(< ?attvalue ?av)
		then;;yes
		(retract ?rmv)
	(retract ?nxt)
	(printout t crlf "next" crlf)
	(assert 
	(Currentgroup 
	(GroupId ?z)
	(SuccessType ?a)
	(SuccessArg ?b)
	(TrueCount ?c)
	(conclusion ?d)
	(answer ?e)
	))
	(modify ?hist (ID $?x ?z))
		else;;no
		
		)
	)
)

)



)

;; change page here
(defrule next_question_orig
?rmv<-(Currentgroup (GroupId ?y))
?nxt<-(next)
(group (GroupId ?y) (answer ?ans))
(Navigation (CriteriaGroupID ?y&~NIL)(CriteriaAnswer ?ans)(DestinationGroupID ?z&~NIL) (AttributeName NIL))
?hist<-(NaviHistory (ID $?x))
?grp<-
(group 
(GroupId ?z)
(SuccessType ?a)
(SuccessArg ?b)
(TrueCount ?c)
(conclusion ?d)
(answer ?e)

)
=>

	(retract ?rmv)
	(retract ?nxt)
	(printout t crlf "next" crlf)
	(assert 
	(Currentgroup 
	(GroupId ?z)
	(SuccessType ?a)
	(SuccessArg ?b)
	(TrueCount ?c)
	(conclusion ?d)
	(answer ?e)
	))
	(modify ?hist (ID $?x ?z))

)

;;remove unesscary previous
(defrule previous_useless
?prev<-(previous)
?hist<-(NaviHistory (ID NIL ?x))
=>
(retract ?prev)
)

;; change page here
(defrule previous_question
?rmv<-(Currentgroup (GroupId ?y))
?prev<-(previous)
?hist<-(NaviHistory (ID $?x ?p ?y))
?grp<-
(group 
(GroupId ?p)
(SuccessType ?a)
(SuccessArg ?b)
(TrueCount ?c)
(conclusion ?d)
(answer ?e)

)
=>
(retract ?rmv)
(retract ?prev)
(printout t crlf "previous" crlf)
(assert 
(Currentgroup 
(GroupId ?p)
(SuccessType ?a)
(SuccessArg ?b)
(TrueCount ?c)
(conclusion ?d)
(answer ?e)
))
(modify ?hist (ID $?x ?p))
)

(defrule next_Conclusion
(Currentgroup (GroupId ?y))
?nxt<-(next)
(Navigation (CriteriaGroupID ?y&~NIL) (DestinationGroupID NIL))
=>
(retract ?nxt)
(printout t crlf "conclusion met" crlf)
(assert (conclusion))
)



;;(mode 1)
;;(defrule test_add
;;?his<-(NaviHistory (ID $?x))
;;=>
;;(modify ?his (ID $?x new))
;;)


(defrule Mode2-Pass
(mode 2)
(mode2criteriaGroup (GroupID ?z&~NIL))
(forall 
	(mode2criteria (Answer ?x) (ID ?y) (GroupID ?z))
	(group (answer ?x) (GroupId ?y))
)
(not (exists
	(mode2criteriaAttribute (Answer No) (GroupID ?z))
))	
=>
(printout t crlf "Satisfied"?z"!" crlf)
(assert (diagnosis (Comment "OMG")))
)

(defrule Mode2-Attribute-Pass
(mode 2)
?crit<- (mode2criteriaAttribute (AttributeName ?an) (AttributeValue ?attvalue) (AttributeCompareType ?at) (Answer NIL))
?att<- (attribute ?an ?av)
=>

(switch ?at
	(case = then 
	
		(if(= ?attvalue ?av)
		then;;yes
			(modify ?crit (Answer Yes))
		else;;no
			(modify ?crit (Answer No))
		)
	)

	(case <= then 
	
		(if(<= ?attvalue ?av)
		then;;yes
			(modify ?crit (Answer Yes))
	
		else;;no
			(modify ?crit (Answer No))
		)
	)
(case >= then 
	
		(if(>= ?attvalue ?av)
		then;;yes
			(modify ?crit (Answer Yes))
	
		else;;no
			(modify ?crit (Answer No))
		)
	)
	(case > then 
	
		(if(> ?attvalue ?av)
		then;;yes
			(modify ?crit (Answer Yes))
		else;;no
			(modify ?crit (Answer No))
		)
	)

	(case < then 
	
		(if(< ?attvalue ?av)
		then;;yes
			(modify ?crit (Answer Yes))
		else;;no
			(modify ?crit (Answer No))	
		)
	)
)

)

