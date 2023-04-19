
export interface Message {
    id: number | string;
    name: string;
    lecturesAttended: number;
    totalLectures: number;
    marks: Record<string, {
        subjectTitle: string;
        totalMarks: number;
        marksObtained: number;
    }>;
}
