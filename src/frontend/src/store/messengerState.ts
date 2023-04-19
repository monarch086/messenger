import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import {Chat, Message} from "../models/chat.model";


export type MessengerState = {
    chats: Chat[],
};

const initialState: MessengerState = {
    chats: [],
};

// immer.js under the hood
export const studentsSlice = createSlice({
    name: 'messenger',
    initialState,
    reducers: {
        setLoading: (state, { payload }: PayloadAction<{ additional: boolean }>) => {
          // state[payload.additional ? 'additionalLoading' : 'initialLoading'] = 'loading';
        },
        setError: (state, { payload }: PayloadAction<{ additional: boolean }>) => {
            // state[payload.additional ? 'additionalLoading' : 'initialLoading'] = 'error';
        },
        dataLoaded: (state, { payload }: PayloadAction<{ additional: boolean, students: Chat[] }>) => {
            /*state[payload.additional ? 'additionalLoading' : 'initialLoading'] = 'loaded';
            if (payload.additional) {
                state.chats.push(...payload.students);
            } else {
                state.chats = payload.students;
            }*/
        },
    },
});

export const { setLoading, setError, dataLoaded } = studentsSlice.actions;

export default studentsSlice.reducer;
