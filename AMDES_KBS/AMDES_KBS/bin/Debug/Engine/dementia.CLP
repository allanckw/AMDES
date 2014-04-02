;;CS4244 Project (Group 8)
;;AMDES Rule Base MODEL
;; Ang Ji Kai (A0073896L)
;; Ang Nickolas (A0077307A)
;; Chong Kok Wei (A0073859M)

(deftemplate Currentgroup
   (slot GroupID (type SYMBOL))
   (slot SuccessType (type SYMBOL) (allowed-symbols AND OR COUNT)) ;;only 3 types, AND / OR / COUNT
   (slot SuccessArg (type INTEGER))
   (slot TrueCount (type INTEGER) (default 0))
   ;;(slot conclusion (type SYMBOL) (default NIL)) moved to navigations
   (slot answer (type SYMBOL) (default No) (allowed-symbols Yes No)))

(deftemplate Currentquestion
   (slot ID (type SYMBOL))
   (slot QuestionText (type STRING))
   (slot answer (type STRING) (default ""))
   (slot GroupID (type SYMBOL)))

;; Final Diagnosis
(deftemplate diagnosis
   (multislot RID (type SYMBOL)))

;; A Group has many questions
(deftemplate group
   (slot GroupID (type SYMBOL))
   (slot SuccessType (type SYMBOL))
   (slot SuccessArg (type INTEGER))
   (slot TrueCount (type INTEGER) (default 0))
   ;;(slot conclusion (type SYMBOL) (default NIL)) moved to navigations
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
   (slot AttributeCompareType (type SYMBOL) (default NIL))
   (slot Answer (type SYMBOL) (default No) (allowed-symbols Yes No)))

(deftemplate NaviHistory
   (multislot ID (type SYMBOL) (default NIL END)))

(deftemplate symptoms
   (slot ID (type SYMBOL))
   (slot symptom (type STRING)))


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

(defrule Assert_CurrentQuestion
   (Currentgroup (GroupID ?y))
   (question (GroupID ?y) (ID ?a) (QuestionText ?b) (answer ?c))
   (not (exists
             (Currentquestion (ID ?a))))
   =>
   (assert (Currentquestion (GroupID ?y) (ID ?a) (QuestionText ?b) (answer ?c))))

(defrule Retract_CurrentQuestion
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
             (symptoms (ID ?id))))
   =>
   (assert (symptoms (symptom ?sym) (ID ?id))))

(defrule Add_Symptom_For_Group_Type_Normal
   (group (answer Yes) (GroupID ?id) (SuccessType AND|OR))
   (groupid-symptoms (GroupID ?id) (symptom ?sym))
   (not (exists
             (symptoms (ID ?id))))
   =>
   (assert (symptoms (symptom ?sym) (ID ?id))))

(defrule Retract-symptoms-groupid-count
   (group (answer Yes) (GroupID ?Noid) (SuccessType COUNT))
   (groupid-symptoms (GroupID ?Noid))
   ?sym <- (symptoms (ID ?Noid))
   =>
   (retract ?sym))

(defrule Retract-symptoms-groupid-not-count
   (group (answer No) (GroupID ?Noid) (SuccessType OR|AND))
   (groupid-symptoms (GroupID ?Noid))
   ?sym <- (symptoms (ID ?Noid))
   =>
   (retract ?sym))
   
(defrule Add_Symptom_For_Question
   (question (answer Yes) (ID ?id))
   (questionid-symptoms (QuestionID ?id) (symptom ?sym))
   (not (exists
             (symptoms (ID ?id))))
   =>
   (assert (symptoms (symptom ?sym) (ID ?id))))

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

(defrule Go_To_Next_Question
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



(defrule Previous_Question
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

(defrule previous_useless
   ?prev <- (previous)
   ?hist <- (NaviHistory (ID NIL ?x END))
   (not (exists
             (diagnosis)))
   =>
   (retract ?prev)
   (printout t "Only 1 group in history, cannot previous any more" crlf)) 

(defrule Retract-symptoms-questionid
   ?sym <- (symptoms (ID ?Noid))
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
   (retract ?rmv)
   (retract ?nxt)
   (assert (diagnosis (RID $?diag)))
   (assert (Currentgroup (GroupID _RESULT) (answer Yes)))
   (modify ?hist (ID $?x _RESULT END)))
   
(defrule Retract_Diagnosis
   (declare (salience 1))
   ;;(mode 1)
   ?p <- (previous)
   ?diag <- (diagnosis)
   =>
   (retract ?p)
   (retract ?diag)
   (assert (previous)))
   
(defrule Stop_assertion_after_Diagnosis
   (declare (salience 1))
   ?n <- (next)
   (exists
        (diagnosis))
   =>
   (retract ?n)
   (printout t "Conclusion Reached, Cannot next anymore" crlf))
  
; ; The initial facts
(deffacts the-facts
(NaviHistory)

)
