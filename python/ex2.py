def exception_error_split():
    print("split Error 발생했습니다.")
    return

def value_error():
    print("올바른 범위의 숫자( 1 ~ 5 ) 가 아닙니다.")
    return

def print_grader_result(students):
    stu_result_sheet = sorted(students, key=lambda x: x[1], reverse=True)
    rank = 1
    idx = 1
    rank_compare = stu_result_sheet[0][1]
    for name, score in stu_result_sheet:
        if rank_compare > score:
            rank_compare = score
            rank = idx

        print("학생: ", name, " 점수: ", score, "점 ", rank, "등")
        idx += 1
    return

def judge_grader(students, answer):
    student_sheet = []
    for name, student_answer in students:
        score = 0
        for stu_ans in zip(student_answer, answer):
             if stu_ans[0] == stu_ans[1]:
                score += 10
        student_sheet.append([name, score])
    return student_sheet

def exception_handling_grader(students):
    student_sheet = []
    try:
        for student in students:
            name, answer_sheet = student.split(",")
            
            if not answer_sheet.isdigit() and len(answer_sheet) != 10:
                value_error()
                return False

            sheet_element = []
            sheet_element.append(name)
            sheet_answer = []
            for answer in answer_sheet:
                answer_to_int = int(answer)
                if not 1 <= answer_to_int <= 5:
                    value_error()
                    return False
                sheet_answer.append(answer_to_int)
            
            sheet_element.append(sheet_answer)
            student_sheet.append(sheet_element)
    except:
        exception_error_split()
        return False

    return student_sheet

def grader(s, a):
    students = exception_handling_grader(s)
    if students == False:
        return

    students_result = judge_grader(students, a)
    print_grader_result(students_result)
    return

# 학생 답
s = ["김갑,3242524212",
"이을,3242524223",
"박병,2242554112",
"최정,4245242315",
"정무,3242524315"]

# 정답지
a = [3,2,4,2,5,2,4,3,1,2]


grader(s, a)
#학생: 정무 점수: 90점 1등
#학생: 김갑 점수: 80점 2등
#학생: 이을 점수: 70점 3등
#학생: 박병 점수: 50점 4등
#학생: 최정 점수: 40점 5등