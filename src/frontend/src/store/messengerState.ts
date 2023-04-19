import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import {Message} from "../models/student.model";

export type MessengerState = {
    messages: Message[],
    initialLoading: 'loading' | 'error' | 'loaded',
    additionalLoading: 'loading' | 'error' | 'loaded',
};

const initialState: MessengerState = {
    messages: [],
    initialLoading: 'loading',
    additionalLoading: 'loaded'
};

// immer.js under the hood
export const studentsSlice = createSlice({
    name: 'messenger',
    initialState,
    reducers: {
        setLoading: (state, { payload }: PayloadAction<{ additional: boolean }>) => {
          state[payload.additional ? 'additionalLoading' : 'initialLoading'] = 'loading';
        },
        setError: (state, { payload }: PayloadAction<{ additional: boolean }>) => {
            state[payload.additional ? 'additionalLoading' : 'initialLoading'] = 'error';
        },
        dataLoaded: (state, { payload }: PayloadAction<{ additional: boolean, students: Message[] }>) => {
            state[payload.additional ? 'additionalLoading' : 'initialLoading'] = 'loaded';
            if (payload.additional) {
                state.messages.push(...payload.students);
            } else {
                state.messages = payload.students;
            }
        },
    },
});

export const { setLoading, setError, dataLoaded } = studentsSlice.actions;

export default studentsSlice.reducer;
