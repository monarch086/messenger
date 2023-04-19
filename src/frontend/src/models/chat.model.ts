
export interface Chat {
    id: number;
    name: string;
    participants: User[];
    messages: Message[];
}

export interface User {
    id: number;
    name: string;
}

export interface Message {
    id: number;
    content: string;
    // date
}
