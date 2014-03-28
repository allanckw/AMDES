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

(deftemplate groupid-symtoms
(slot GroupID (type SYMBOL))
(slot symtom (type STRING)))

(deftemplate questionid-symtoms
(slot QuestionID (type SYMBOL))
(slot symtom (type STRING)))

(deftemplate symtoms
(slot symtom (type STRING))
(slot ID (type SYMBOL))
)


; ; Final Diagnosis
(deftemplate diagnosis
(multislot RID (type SYMBOL))
)

; ; Sign of a symptom
(deftemplate NaviHistory
(multislot ID (type SYMBOL) (default NIL END)))

; ; QuestionQuestionQuestion
(deftemplate question
(slot Id (type SYMBOL))
(slot QuestionText (type STRING))
(slot answer (type SYMBOL) (default NIL))
(slot GroupId (type SYMBOL))
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
(slot NavigationID (type SYMBOL) )
(slot DestinationGroupID (type SYMBOL) (default NIL))
;;result ID
(multislot RID (type SYMBOL) (default NIL))
)

;;Question 
(deftemplate NaviChildCritQ
(slot NavigationID (type SYMBOL) ) ;;NAVID
(slot CriteriaGroupID (type SYMBOL) (default NIL)) ;;Group ID for TRUE
(slot CriteriaAnswer (type SYMBOL) (default No)) 
)

(deftemplate NaviChildCritA
(slot NavigationID (type SYMBOL) )
(slot AttributeName (type SYMBOL) (default NIL))
(slot AttributeValue)
(slot AttributeCompareType (type SYMBOL) (default NIL)) ;;==, >=, >, <, <=, eq for string
(slot Answer (type SYMBOL) (default No))
)

;;so the forall can work
(deftemplate NaviChildCritASTUB
(slot Answer (type SYMBOL) (default Yes))
)

; ; The initial facts
(deffacts the-facts
(NaviHistory)
(NaviChildCritASTUB)
)

(defrule Pass-And
?CUR_Group <- (group (GroupId ?z&~NIL) (SuccessType AND) (answer ~Yes))
(not (exists
	
	(question (Id ?a) (QuestionText ?b) (answer No) (GroupId ?z))
))
=>
;;(printout t crlf "Satisfied "?z"!" crlf)
(modify ?CUR_Group (answer Yes))
)

(defrule Fail-And
?CUR_Group <- (group (GroupId ?z&~NIL) (SuccessType AND) (answer Yes))
(exists
	
	(question (Id ?a) (QuestionText ?b) (answer No) (GroupId ?z))
)
=>
;;(printout t crlf "Satisfied "?z"!" crlf)
(modify ?CUR_Group (answer No))
)

(defrule Pass-OR
?CUR_Group <- (group (GroupId ?z&~NIL) (SuccessType OR) (answer ~Yes))
(exists
	
	(question (Id ?a) (QuestionText ?b) (answer Yes) (GroupId ?z))
)
=>
;;(printout t crlf "Satisfied "?z"!" crlf)
(modify ?CUR_Group (answer Yes))
)

(defrule Fail-OR
?CUR_Group <- (group (GroupId ?z&~NIL) (SuccessType OR) (answer Yes))
(not (exists
	
	(question (Id ?a) (QuestionText ?b) (answer Yes) (GroupId ?z))
))
=>
;;(printout t crlf "Satisfied "?z"!" crlf)
(modify ?CUR_Group (answer No))
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



;;remove unesscary previous
;;only 1 question in history cant previous anymore
(defrule previous_useless
?prev<-(previous)
?hist<-(NaviHistory (ID NIL ?x END))
=>
(retract ?prev)
)

;; change page here
(defrule previous_question
?rmv<-(Currentgroup (GroupId ?y))
?prev<-(previous)
?hist<-(NaviHistory (ID $?x ?p ?y END))
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
(modify ?hist (ID $?x ?p END))
)
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;;navigate next question

;;initial question
;; change page here
(defrule Initial_question
(mode 1)

(Navigation (NavigationID ?naviID) (DestinationGroupID ?z&~NIL) (RID NIL))
(forall
(NaviChildCritQ (NavigationID ?naviID)(CriteriaGroupID ?CGID) (CriteriaAnswer ?CA))
(group (GroupId ?CGID) (answer ?CA))
(NaviHistory (ID $?START ?CGID $?END))
)
(not (exists (Currentgroup)))
(forall
(NaviChildCritA (NavigationID ?naviID) (Answer ?yes))
(NaviChildCritASTUB (Answer ?yes))
)
?hist<-(NaviHistory (ID $?x END))
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
	(assert 
	(Currentgroup 
	(GroupId ?z)
	(SuccessType ?a)
	(SuccessArg ?b)
	(TrueCount ?c)
	(conclusion ?d)
	(answer ?e)
	))
	(modify ?hist (ID $?x ?z END))

)

;; change page here
(defrule next_question
(mode 1)
?rmv<-(Currentgroup (GroupId ?y))
?nxt<-(next)
(Navigation (NavigationID ?naviID) (DestinationGroupID ?z&~NIL) (RID NIL))
(forall
(NaviChildCritQ (NavigationID ?naviID)(CriteriaGroupID ?CGID) (CriteriaAnswer ?CA))
(group (GroupId ?CGID) (answer ?CA))
(NaviHistory (ID $?START ?CGID $?END))
)

(forall
(NaviChildCritA (NavigationID ?naviID) (Answer ?yes))
(NaviChildCritASTUB (Answer ?yes))
)
?hist<-(NaviHistory (ID $?x END))
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
	(modify ?hist (ID $?x ?z END))

)

;; Navigation to a conclusion
(defrule next_question
(mode 1)
?rmv<-(Currentgroup (GroupId ?y))
?nxt<-(next)
(Navigation (NavigationID ?naviID) (DestinationGroupID NIL) (RID ?diag))
(forall
(NaviChildCritQ (NavigationID ?naviID)(CriteriaGroupID ?CGID) (CriteriaAnswer ?CA))
(group (GroupId ?CGID) (answer ?CA))
(NaviHistory (ID $?START ?CGID $?END))
)

(forall
(NaviChildCritA (NavigationID ?naviID) (Answer ?yes))
(NaviChildCritASTUB (Answer ?yes))
)
?hist<-(NaviHistory (ID $?x END))
=>

	(retract ?rmv)
	(retract ?nxt)
	(assert (diagnosis (RID ?diag)))
	
	

)

;;;;;;;;;self attribute assertion here
(defrule next_question
(attribute ?attribu ?value)
?target<-(NaviChildCritA  (AttributeName ?attribu) (AttributeValue ?cvalue) (AttributeCompareType ?at) (Answer ?ans))
=>


	(switch ?at
	(case = then 
	
		(if(= ?cvalue ?value)
		then;;yes
			
			(switch ?ans
				(case Yes then 
				)
				(case No then 	
					(modify ?target (Answer Yes))
				)
			)


		else;;no
			(switch ?ans
				(case Yes then 
					(modify ?target (Answer No))
				)
				(case No then 	
					
				)
			)
		)
	)

	(case <= then 
	
		(if(<= ?cvalue ?value)
		then;;yes
			
			(switch ?ans
				(case Yes then 
				)
				(case No then 	
					(modify ?target (Answer Yes))
				)
			)


		else;;no
			(switch ?ans
				(case Yes then 
					(modify ?target (Answer No))
				)
				(case No then 	
					
				)
			)
		)
	)
	(case >= then 
	
		(if(>= ?cvalue ?value)
		then;;yes
			
			(switch ?ans
				(case Yes then 
				)
				(case No then 	
					(modify ?target (Answer Yes))
				)
			)


		else;;no
			(switch ?ans
				(case Yes then 
					(modify ?target (Answer No))
				)
				(case No then 	
					
				)
			)
		)
	)
	(case > then 
	
		(if(> ?cvalue ?value)
		then;;yes
			
			(switch ?ans
				(case Yes then 
				)
				(case No then 	
					(modify ?target (Answer Yes))
				)
			)


		else;;no
			(switch ?ans
				(case Yes then 
					(modify ?target (Answer No))
				)
				(case No then 	
					
				)
			)
		)
	)

	(case < then 
	
		(if(< ?cvalue ?value)
		then;;yes
			
			(switch ?ans
				(case Yes then 
				)
				(case No then 	
					(modify ?target (Answer Yes))
				)
			)


		else;;no
			(switch ?ans
				(case Yes then 
					(modify ?target (Answer No))
				)
				(case No then 	
					
				)
			)
		)
	)


)
)

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
(defrule CurrentQuestion-retract
(Currentgroup (GroupId ?y))
?question<-(Currentquestion (GroupId ?x&~?y))
=>
(retract ?question)
)


(defrule CurrentQuestion-assert
(Currentgroup (GroupId ?y))
(question (GroupId ?y) (Id ?a) (QuestionText ?b) (answer ?c))
(not 

(exists
 (Currentquestion (Id ?a))
)
)
=>
(assert (Currentquestion (GroupId ?y) (Id ?a) (QuestionText ?b) (answer ?c)))
)


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
(assert (diagnosis (RID OMG)))
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

(defrule Add-symtoms-groupid
(group (answer Yes) (GroupId ?id) )
(groupid-symtoms (GroupID ?id) (symtom ?sym))
(not (exists (symtoms (ID ?id))))
=>
(assert (symtoms (symtom ?sym) (ID ?id)))

)

(defrule Retract-symtoms-groupid

(group (answer No) (GroupId ?Noid) )
(groupid-symtoms (GroupID ?Noid))
?sym <- (symtoms (ID ?Noid))
=>
(retract ?sym)
;;(printout t crlf "ans:"?sym crlf)
)

(defrule Add-symtoms-questionid
(question (answer Yes) (Id ?id) )
(questionid-symtoms (QuestionID ?id) (symtom ?sym))
(not (exists (symtoms (ID ?id))))
=>
(assert (symtoms (symtom ?sym) (ID ?id)))

)

(defrule Retract-symtoms-questionid

(question (answer Yes) (Id ?Noid) )
(questionid-symtoms (QuestionID ?Noid))
?sym <- (symtoms (ID ?Noid))
=>
(retract ?sym)
;;(printout t crlf "ans:"?sym crlf)
)


