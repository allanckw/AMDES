;;CS4244 Project (Group 8)
;; AMDES Rule Base MODEL
;; Ang Ji Kai (A0073896L)
;; Ang Nickolas (A0077307A)
;; Chong Kok Wei (A0073859M)


(defmodule MAIN (export deftemplate initial-fact))
(defrule MAIN::Change-Focus
		=>
		(set-strategy depth) 
		(printout t "change strategy to depth" crlf)
)

(deftemplate Currentgroup
   (slot GroupID (type SYMBOL))
   (slot SuccessType (type SYMBOL) (allowed-symbols AND OR COUNT)) ;;only 3 types, AND / OR / COUNT
   (slot SuccessArg (type INTEGER))
   (slot TrueCount (type INTEGER) (default 0))
   (slot answer (type SYMBOL) (default No) (allowed-symbols Yes No)))

(deftemplate Currentquestion
   (slot GroupID (type SYMBOL))
   (slot ID (type SYMBOL))
   (slot QuestionText (type STRING))
   (slot answer (type SYMBOL) (default No) (allowed-symbols Yes No)))

;; Final Diagnosis
(deftemplate diagnosis
   (multislot RID (type SYMBOL))
   (slot Comment (type STRING)))

;; A Group has many questions
(deftemplate group
   (slot GroupID (type SYMBOL))
   (slot SuccessType (type SYMBOL) (allowed-symbols AND OR COUNT)) ;;only 3 types, AND / OR / COUNT
   (slot SuccessArg (type INTEGER))
   (slot TrueCount (type INTEGER) (default 0))
   (slot answer (type SYMBOL) (default No) (allowed-symbols Yes No)))

;;A Group has a symptom
(deftemplate groupid-symptoms
   (slot GroupID (type SYMBOL))
   (slot symptom (type STRING)))

(deftemplate question
   (slot ID (type SYMBOL))
   (slot QuestionText (type STRING))
   (slot answer (type SYMBOL) (default No) (allowed-symbols Yes No))
   (slot GroupID (type SYMBOL)))

;;A question has a symptom but can be null
(deftemplate questionid-symptoms
	(slot GroupID (type SYMBOL))
    (slot QuestionID (type SYMBOL))
    (slot symptom (type STRING)))

;; navigation 
;; A navigation element has Destination OR Diagnosis ID (RID = RuleID for short)
(deftemplate Navigation
   (slot NavigationID (type SYMBOL))
   (slot DestinationGroupID (type SYMBOL) (default NIL))
   (multislot RID (type SYMBOL) (default NIL)))

;;Child Criteria Question, to go the next question what CriteriaGroup should be true
(deftemplate NaviChildCritQuestion
   (slot NavigationID (type SYMBOL))
   (slot CriteriaGroupID (type SYMBOL) (default NIL))
   (slot CriteriaAnswer (type SYMBOL) (default No) (allowed-symbols Yes No)))

;;Child Criteria Question, to go the next question what are the attributes required and the compare type
(deftemplate NaviChildCritAttribute
   (slot NavigationID (type SYMBOL))
   (slot AttributeName (type SYMBOL) (default NIL))
   (slot AttributeValue)
   (slot AttributeCompareType (type SYMBOL) (default NIL) )
   (slot Answer (type SYMBOL) (default No) (allowed-symbols Yes No)))

(deftemplate NaviHistory
   (multislot ID (type SYMBOL) (default NIL END)))

(deftemplate symptoms
   (slot GroupID (type SYMBOL))
   (slot QuestionID (type SYMBOL) (default NIL))
   (slot symptom (type STRING)))

(deftemplate candidate-diagnosis
	(slot RID (type SYMBOL))
	(slot Header (type STRING))
	(slot Comment (type STRING))
	(slot Link (type STRING) (default "")))
   
 (defrule NavigationError
	?nav <-(Navigation (NavigationID ?naviID) (DestinationGroupID ?z&~NIL) (RID ?y&~NIL))
	?navAttr <- (NaviChildCritAttribute (NavigationID ?naviID))
	?navQngrp <- (NaviChildCritQuestion (NavigationID ?naviID))
	=>
	(printout t "Error In Navigations ID: " ?naviID  ", please check your navigation point" crlf)
	(retract ?nav)
	(retract ?navAttr)
	(retract ?navQngrp))
	
(defrule NavigationError
	?nav <-(Navigation (NavigationID ?naviID) (DestinationGroupID ?z&NIL) (RID ?y&NIL))
	?navAttr <- (NaviChildCritAttribute (NavigationID ?naviID))
	?navQngrp <- (NaviChildCritQuestion (NavigationID ?naviID))
	=>
	(printout t "Error In Navigations ID: " ?naviID  ", please check your navigation point" crlf)
	(retract ?nav)
	(retract ?navAttr)
	(retract ?navQngrp))
   
(defrule Initial_Question
   ;;(mode 1)
   (Navigation (NavigationID ?naviID) (DestinationGroupID ?z&~NIL) (RID NIL))
   (not (exists
             (NaviChildCritQuestion (NavigationID ?naviID))))
   (not (exists
             (NaviChildCritAttribute (NavigationID ?naviID) (Answer No))))
   (not (exists
             (Currentgroup)))
   ?hist <- (NaviHistory (ID $?x END))
   ?grp <- (group (GroupID ?z) (SuccessType ?a) (SuccessArg ?b) (TrueCount ?c)  (answer ?e))
   =>
   (assert (Currentgroup (GroupID ?z) (SuccessType ?a) (SuccessArg ?b) (TrueCount ?c)  (answer ?e)))
   (modify ?hist (ID $?x ?z END)))

(defrule Go_To_Next_QuestionGrp
   ;;(mode 1)
   (not (exists
             (diagnosis)))
   ?rmv <- (Currentgroup (GroupID ?y))
   ?nxt <- (next)
   (Navigation (NavigationID ?naviID) (DestinationGroupID ?z&~NIL) (RID NIL))
   (exists
        (NaviChildCritQuestion (NavigationID ?naviID)))
   (forall
        (NaviChildCritQuestion (NavigationID ?naviID) (CriteriaGroupID ?CGID) (CriteriaAnswer ?CA))
        (group (GroupID ?CGID) (answer ?CA))
        (NaviHistory (ID $?START ?CGID $?END)))
   (not (exists
             (NaviChildCritAttribute (NavigationID ?naviID) (Answer No))))
   ?hist <- (NaviHistory (ID $?x END))
   ?grp <- (group (GroupID ?z) (SuccessType ?a) (SuccessArg ?b) (TrueCount ?c) (answer ?e))
   =>

   (retract ?rmv)
   (retract ?nxt)
   (printout t crlf "next" crlf)
   (assert (Currentgroup (GroupID ?z) (SuccessType ?a) (SuccessArg ?b) (TrueCount ?c) (answer ?e)))
   (modify ?hist (ID $?x ?z END)))

(defrule Go_To_Previous_QuestionGrp
   ?prev <- (previous)

   (not (exists
             (diagnosis)))
   ?rmv <- (Currentgroup (GroupID ?y))
   ?hist <- (NaviHistory (ID $?x ?p ?y END))
   ?grp <- (group (GroupID ?p) (SuccessType ?a) (SuccessArg ?b) (TrueCount ?c) (answer ?e))
   =>
   (retract ?rmv)
   (retract ?prev)
   (printout t crlf "previous" crlf)
   (assert (Currentgroup (GroupID ?p) (SuccessType ?a) (SuccessArg ?b) (TrueCount ?c) (answer ?e)))
   (modify ?hist (ID $?x ?p END)))

(defrule Assert_CurrentQuestion
   (not (exists
             (diagnosis)))
   (Currentgroup (GroupID ?y))
   (question (GroupID ?y) (ID ?a) (QuestionText ?b) (answer ?c))
   (not (exists
             (Currentquestion (ID ?a))))
   =>
   (assert (Currentquestion (GroupID ?y) (ID ?a) (QuestionText ?b) (answer ?c)))
   (printout t "Question ID: " ?a ": " ?b crlf))

(defrule Retract_CurrentQuestion
   (not (exists
             (diagnosis)))
   (Currentgroup (GroupID ?y))
   ?question <- (Currentquestion (GroupID ?x&~?y))
   =>
   (retract ?question))

(defrule Change-Answer
   ?f1 <- (choice ?ID ?x)
   ?f2 <- (question (ID ?ID) (answer ?y) (GroupID ?z))
   ?f3 <- (group (GroupID ?z) (TrueCount ?count))
   =>
   (switch ?y
      (case NIL then
         (if (eq Yes ?x)
            then
            (modify ?f3 (TrueCount (+ ?count 1)))
            else))
      (case Yes then
         (if (eq Yes ?x)
            then
            else
            (modify ?f3 (TrueCount (- ?count 1)))))
      (case No then
         (if (eq Yes ?x)
            then
            (modify ?f3 (TrueCount (+ ?count 1)))
            else)))
   (retract ?f1)
   (modify ?f2 (answer ?x)))

(defrule Add_Symptom_For_Group_Type_Count ;;for count type, only failing the threshold -> fail
   (group (answer No) (GroupID ?id) (SuccessType COUNT))
   (groupid-symptoms (GroupID ?id) (symptom ?sym))
   (not (exists
             (symptoms (GroupID ?id))))
   =>
   (assert (symptoms (symptom ?sym) (GroupID ?id))))

(defrule Add_Symptom_For_Group_Type_Normal
   (group (answer Yes) (GroupID ?id) (SuccessType AND|OR))
   (groupid-symptoms (GroupID ?id) (symptom ?sym))
   (not (exists
             (symptoms (GroupID ?id))))
   =>
   (assert (symptoms (symptom ?sym) (GroupID ?id))))

(defrule Retract-symptoms-groupid-count
   (group (answer Yes) (GroupID ?Noid) (SuccessType COUNT))
   (groupid-symptoms (GroupID ?Noid))
   ?sym <- (symptoms (GroupID ?Noid))
   =>
   (retract ?sym))

(defrule Retract-symptoms-groupid-not-count
   (group (answer No) (GroupID ?Noid) (SuccessType OR|AND))
   (groupid-symptoms (GroupID ?Noid))
   ?sym <- (symptoms (GroupID ?Noid))
   =>
   (retract ?sym))
   
(defrule Add_Symptom_For_Question
   (question (answer Yes) (ID ?id))
   (questionid-symptoms  (symptom ?sym) (GroupID ?grp) (QuestionID ?id))
   (not (exists
             (symptoms (GroupID ?grp) (QuestionID ?id))))
   =>
   (assert (symptoms (GroupID ?grp) (QuestionID ?id) (symptom ?sym))))

(defrule Next_Question_Attribute_Check
   (attribute ?attribu ?value)
   ?target <- (NaviChildCritAttribute (AttributeName ?attribu) (AttributeValue ?cvalue) (AttributeCompareType ?at) (Answer ?ans))
   =>
   (switch ?at
      (case = then
         (if (= ?cvalue ?value)
            then
            (switch ?ans
               (case Yes then)
               (case No then
                  (modify ?target (Answer Yes))))
            else
            (switch ?ans
               (case Yes then
                  (modify ?target (Answer No)))
               (case No then))))
      (case <= then
         (if (<= ?value ?cvalue)
            then
            (switch ?ans
               (case Yes then)
               (case No then
                  (modify ?target (Answer Yes))))
            else
            (switch ?ans
               (case Yes then
                  (modify ?target (Answer No)))
               (case No then))))
      (case >= then
         (if (>= ?value ?cvalue)
            then
            (switch ?ans
               (case Yes then)
               (case No then
                  (modify ?target (Answer Yes))))
            else
            (switch ?ans
               (case Yes then
                  (modify ?target (Answer No)))
               (case No then))))
      (case > then
         (if (> ?value ?cvalue)
            then
            (switch ?ans
               (case Yes then)
               (case No then
                  (modify ?target (Answer Yes))))
            else
            (switch ?ans
               (case Yes then
                  (modify ?target (Answer No)))
               (case No then))))
      (case < then
         (if (< ?value ?cvalue)
            then
            (switch ?ans
               (case Yes then)
               (case No then
                  (modify ?target (Answer Yes))))
            else
            (switch ?ans
               (case Yes then
                  (modify ?target (Answer No)))
               (case No then))))))



(defrule Pass_AND_Type_Group 
   ?CUR_Group <- (group (GroupID ?z&~NIL) (SuccessType AND) (answer ~Yes))
   (not (exists
             (question (ID ?a) (QuestionText ?b) (answer No) (GroupID ?z))))
   =>
   (modify ?CUR_Group (answer Yes)))

(defrule Pass_OR_Type_Group 
   ?CUR_Group <- (group (GroupID ?z&~NIL) (SuccessType OR) (answer ~Yes))
   (exists
        (question (ID ?a) (QuestionText ?b) (answer Yes) (GroupID ?z)))
   =>
   (modify ?CUR_Group (answer Yes)))
   
(defrule Fail_AND_Type_Group 
   ?CUR_Group <- (group (GroupID ?z&~NIL) (SuccessType AND) (answer Yes))
   (exists
        (question (ID ?a) (QuestionText ?b) (answer No) (GroupID ?z))) ;;if there exist a no, fail
   =>
   (modify ?CUR_Group (answer No)))

(defrule Fail_AND_Type_Group 
   ?CUR_Group <- (group (GroupID ?z&~NIL) (SuccessType OR) (answer Yes))
   (not (exists
             (question (ID ?a) (QuestionText ?b) (answer Yes) (GroupID ?z)))) ;;if there exist a yes, pass
   =>
   (modify ?CUR_Group (answer No)))
   
(defrule Pass_COUNT_NO_to_YES
   ?CUR_Group <- (group (GroupID ?z&~NIL) (SuccessType COUNT) (TrueCount ?b) (SuccessArg ?c) (answer ~Yes))
   (test (>= ?b ?c))
   =>
   (modify ?CUR_Group (answer Yes)))

(defrule Pass_COUNT_YES_to_NO
   ?CUR_Group <- (group (GroupID ?z&~NIL) (SuccessType COUNT) (TrueCount ?b) (SuccessArg ?c) (answer ~No))
   (test (< ?b ?c))
   =>
   (modify ?CUR_Group (answer No)))



(defrule previous_useless
   ?prev <- (previous)
   ?hist <- (NaviHistory (ID NIL ?x END))
   (not (exists
             (diagnosis)))
   =>
   (retract ?prev)
   (printout t "Only 1 group in history, cannot previous any more" crlf)) 


   
(defrule Retract-symptoms-questionid
   ?sym <- (symptoms (QuestionID ?Noid&~NIL) (GroupID ?grp))
   (question (answer No) (ID ?Noid))
   (questionid-symptoms (QuestionID ?Noid))
   =>
   (retract ?sym))

(defrule Assert_Diagnosis
   (declare (salience 1))
   ?nxt <- (next)
   (not (exists
             (diagnosis)))
   ;;(mode 1)
   ?rmv <- (Currentgroup (GroupID ?y))
   (Navigation (NavigationID ?naviID) (DestinationGroupID NIL) (RID $?diag))
   ?hist <- (NaviHistory (ID $?x END))
   (exists
        (NaviChildCritQuestion (NavigationID ?naviID)))
   (forall
        (NaviChildCritQuestion (NavigationID ?naviID) (CriteriaGroupID ?CGID) (CriteriaAnswer ?CA))
        (group (GroupID ?CGID) (answer ?CA))
        (NaviHistory (ID $?START ?CGID $?END)))
   (not (exists
             (NaviChildCritAttribute (NavigationID ?naviID) (Answer No))))
	

   =>
	(loop-for-count (?cnt (length$ $?diag)) ;;Used as pointers
		(printout t (nth$ ?cnt ?diag) crlf)
		(assert (Conclusion (nth$ ?cnt ?diag)))	
	)
   (retract ?rmv)
   (retract ?nxt)
   (assert (diagnosis (RID $?diag)))
   (assert (Currentgroup (GroupID _RESULT) (answer Yes)))
   (modify ?hist (ID $?x _RESULT END)))

(defrule print-R  ;;Print Diagnosis 
		(candidate-diagnosis (RID ?diag) (Header ?diag.h) (Comment ?diag.c) (Link ?diag.l))
		(Conclusion ?diag)
		=>
		(printout t ?diag.h crlf)
		(printout t ?diag.c crlf)
		(printout t ?diag.l crlf)

)
   
(defrule Retract_Diagnosis
   (declare (salience 1))
   ?p <- (previous)
   ?diag <- (diagnosis)
   =>   
   (retract ?p)
   (retract ?diag)
   (assert (previous)))

(defrule Retract_Conclusions
   (declare (salience 1))
   ?diag <- (diagnosis)
   ?p <- (previous)
   ?conc <- (Conclusion ?c)
   =>
   (retract ?conc)
)
   
(defrule Stop_assertion_after_Diagnosis
   (declare (salience 1))
   ?n <- (next)
   (exists
        (diagnosis))
   =>
   (retract ?n)
   (printout t "Conclusion Reached, Cannot next anymore" crlf))
  
(defrule Graceful_Exit1 ;;Special case no conclusion but invalid program behavior
    (declare (salience 1))	
	?hist2 <- (NaviHistory (ID $?x ?y ?y END))
	?n <- (next)
	=>
   (retract ?n)
   (assert (Currentgroup (GroupID _RESULT) (answer Yes)))
   (assert (diagnosis (RID R-99) (Comment "Going back to the same current section!" )))
   (printout t "Going back to the same current question group / section!" crlf)
)

(defrule Graceful_Exit2 ;;Special case no conclusion but invalid program behavior
	 ?n <- (next)
	 ?rmv <- (Currentgroup (GroupID ?y))
	 (not(exists
			(NaviChildCritQuestion (CriteriaGroupID ?y))))
	 =>
	(retract ?n)
	(retract ?rmv)
	(assert (Currentgroup (GroupID _RESULT) (answer Yes)))
    (assert (diagnosis (RID R-99) (Comment "No navigation point defined!") ))
	(printout t "No navigation point defined!" crlf)
)

(defrule Graceful_Exit3 ;;Special case no conclusion but invalid program behavior
    (not(exists
			(Navigation)))
	?n <- (next)
	=>
   (retract ?n)
   (assert (Currentgroup (GroupID _RESULT) (answer Yes)))
   (assert (diagnosis (RID R-99) (Comment "No Navigation has been defined!" )))
   (printout t "No Navigation has been defined!" crlf)
)


; ; The initial facts
(deffacts the-facts
(NaviHistory)

)


;;;;;;;;;;;;;;;;;;;;;;DecisionTree;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
(defmodule DecisionTree (import MAIN deftemplate initial-fact)) 
;;Compatiability issue from 6.2.3 (IVLE Copy, System will complain no initial data)

(defrule DecisionTree::Change-Focus
		=>
		(set-strategy complexity)
		(printout t "change strategy to complexity")
)

(deftemplate DecisionTree::LeafSet ;;conjunctive set of the disjunctive normal form
(slot LeafID )
(slot Result (type SYMBOL) (default NIL))
(slot Total (type INTEGER) (default 0))

)

(deftemplate DecisionTree::clause ;;
(slot LeafID )
(slot CompareType )
(slot CompareValue) ;;eg CompareValue > attributevalue
(slot CompareAttributeName)
(slot Result (type SYMBOL) (default NIL))

)

(deftemplate DecisionTree::Conclusion ;;conclusion if this set resolve true
(slot LeafID )
(slot Diagnosis (type SYMBOL));;maybe this can change to RID or result ID
(slot Agree)
(slot DisAgree)
)

(deftemplate DecisionTree::Answer;;
(slot Diagnosis (type SYMBOL));;maybe this can change to RID or result ID
(slot Agree)
(slot DisAgree)
)

(deftemplate DecisionTree::Attribute ;;
(slot Name )
(slot Value )
)



; ; The initial facts
(deffacts DecisionTree::the-facts
	;;(Attribute (Name Age) (Value 44))
	;;(Attribute (Name test) (Value Yes))

	;;(LeafSet (LeafID 1))
	;;(clause (LeafID 1) (CompareType eq ) (CompareValue Yes) (CompareAttributeName test) )
	;;;(clause (LeafID 1) (CompareType == ) (CompareValue 44) (CompareAttributeName Age) )
	;;(Conclusion (LeafID 1) (Diagnosis D1))
	;;(Conclusion (LeafID 1) (Diagnosis D2))

;;(assert (Attribute (Name TT4) (Value 20)))

)

;;;;;;;;;;;
;;insert diagnosis
(defrule DecisionTree::Diagnosis-finalize
(Conclusion (LeafID ?ID) (Diagnosis ?D) (Agree ?a) (DisAgree ?da))
(LeafSet (LeafID ?ID) (Result Pass) )
=>
(assert (Answer (Diagnosis ?D) (Agree ?a) (DisAgree ?da)))
)



(defrule DecisionTree::Process-Clause

?crit<-(clause (LeafID ?id) (CompareType ?CT) (CompareValue ?CV) (CompareAttributeName ?CAN) (Result NIL) )
(Attribute (Name ?CAN) (Value ?V))
=>


(switch ?CT

	(case eq then 
	
		(if(eq ?CV ?V)
		then;;yes
			(modify ?crit (Result Pass))
			
		else;;no
			(modify ?crit (Result Fail))
			(assert (Fail ?id))
		)
	)
	(case == then 
	
		(if(eq ?CV ?V)
		then;;yes
			
			(modify ?crit (Result Pass))
		else;;no
			(modify ?crit (Result Fail))
			(assert (Fail ?id))
		)
	)

	(case = then 
	
		(if(eq ?CV ?V)
		then;;yes
			
			(modify ?crit (Result Pass))
		else;;no
			(modify ?crit (Result Fail))
			(assert (Fail ?id))
		)
	)

	(case <= then 
	
		(if(<= ?CV ?V)
		then;;yes
			
			(modify ?crit (Result Pass))
	
		else;;no
			(modify ?crit (Result Fail))
			(assert (Fail ?id))
		)
	)
	(case >= then 
	
		(if(>= ?CV ?V)
		then;;yes
			(assert (Pass ?id))
			(modify ?crit (Result Pass))
	
		else;;no
			(modify ?crit (Result Fail))
		)
	)
	(case > then 
	
		(if(> ?CV ?V)
		then;;yes
			(assert (Pass ?id))
			(modify ?crit (Result Pass))
		else;;no
			(modify ?crit (Result Fail))
		)
	)

	(case < then 
	
		(if(< ?CV ?V)
		then;;yes
			(assert (Pass ?id))
			(modify ?crit (Result Pass))
		else;;no
			(modify ?crit (Result Fail))	
		)
	)
);;end of switch

)

;;;;;;;;;;
(defrule DecisionTree::Fail-LeafSet
;;(salience 1)
?Leaf <- (LeafSet (LeafID ?ID) (Result NIL))
(exists
	
	(clause (LeafID ?ID) (Result Fail) )

)
=>
(modify ?Leaf (Result Fail))
)

(defrule DecisionTree::Pass-LeafSet
;;(salience 1)
?Leaf <- (LeafSet (LeafID ?ID) (Result NIL))

	(not(exists
		(clause (LeafID ?ID) (Result Fail) )
	))

=>
(modify ?Leaf (Result Pass))
)