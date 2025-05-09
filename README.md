#  Course Management API

This API provides functionality for managing Courses, Assignments, Labs, and Notes in an academic setting.

##  Base URLs

- **Courses:** `/api/courses`
- **Assignments:** `/api/assignments`
- **Labs:** `/api/labs`
- **Notes:** `/api/notes`

---

##  Courses

### ➕ Create Course

**POST** `/api/courses/create`

#### Request Body
```json
{
  "courseName": "Computer Science",
  "section": "A",
  "semester": 6,
  "courseCode": "CS601",
  "faculty": "Dr. John Doe",
  "assignments": ["<assignmentId1>", "<assignmentId2>"],
  "labs": ["<labId1>", "<labId2>"],
  "notes": ["<noteId1>", "<noteId2>"],
  "notices": [
    {
      "email": "faculty@example.com",
      "notice": "Exam postponed",
      "date": "2025-05-09T12:00:00Z"
    }
  ]
}
```

### Get Course by ID

**GET** `/api/courses/get_course/{id}`

### Get All Courses

**GET** `/api/courses/get_all_courses`

### Delete Course

**DELETE** `/api/courses/delete/{id}`

---

## Assignments

### ➕ Create Assignment

**POST** `/api/assignments/create`

#### Request Body
```json
{
  "assignmentName": "Final Project Report",
  "assignmentNo": 3,
  "details": "Prepare and submit your final year project report.",
  "data": ["file_url_1", "file_url_2"],
  "deadline": "2025-05-20T23:59:59Z",
  "uploader": {
    "name": "Dr. John Doe",
    "email": "johndoe@example.com",
    "image": "https://example.com/images/john.png"
  },
  "courseId": "663c6b7fa12e4b9f0c5e3084"
}
```

### Get Assignments by Course

**GET** `/api/assignments/get_assignments/{courseId}`

### Get Specific Assignment

**GET** `/api/assignments/get_assignment/{assignmentId}`

### Update Assignment

**PUT** `/api/assignments/update_assignment/{assignmentId}`

### Delete Assignment

**DELETE** `/api/assignments/delete/{courseId}/{assignmentId}`

---

## Labs

### ➕ Create Lab

**POST** `/api/labs/create`

#### Request Body
```json
{
  "labName": "Operating Systems Lab",
  "labNo": 2,
  "details": "Implementing CPU scheduling algorithms in C.",
  "courseId": "663c6b7fa12e4b9f0c5e3084",
  "data": [
    "https://example.com/labs/os_lab_2_instructions.pdf",
    "https://example.com/labs/os_lab_2_template.zip"
  ],
  "deadline": "2025-05-20T23:59:59Z",
  "uploader": {
    "name": "Dr. John Doe",
    "email": "johndoe@example.com",
    "image": "https://example.com/images/john.png"
  }
}
```

### Get Labs by Course

**GET** `/api/labs/get_labs/{courseId}`

### Get Specific Lab

**GET** `/api/labs/get_lab/{labId}`

### Update Lab

**PUT** `/api/labs/update_lab/{labId}`

### Delete Lab

**DELETE** `/api/labs/delete/{courseId}/{labId}`

---

## Notes

### Create Note

**POST** `/api/notes/create`

#### Request Body
```json
{
  "title": "Introduction to MongoDB",
  "description": "This note covers the basics of MongoDB, including CRUD",
  "data": [
    "https://example.com/notes/mongodb_intro.pdf",
    "https://example.com/notes/mongodb_intro_notes.zip"
  ],
  "uploader": {
    "name": "Dr. Jane Smith",
    "email": "janesmith@example.com",
    "image": "https://example.com/images/jane.png"
  },
  "courseId": "663c6b7fa12e4b9f0c5e3084",
  "createdAt": "2025-05-10T12:00:00Z"
}
```

### Get Notes by Course

**GET** `/api/notes/get_notes/{courseId}`

### Get Specific Note

**GET** `/api/notes/get_note/{noteId}`

### Update Note

**PUT** `/api/notes/update_note/{noteId}`

### Delete Note

**DELETE** `/api/notes/delete/{courseId}/{noteId}`

