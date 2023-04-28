
export interface Chat {
    id: number;
    title: string;
    participants?: User[];
    messages: Message[];
    other?: number; // id of other participant
}

export interface User {
    id: number;
    name: string;
}

export interface Message {
    id: number;
    content: string;
    author?: string;
    mine?: boolean;
    // date
}
