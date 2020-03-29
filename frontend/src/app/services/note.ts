export class Note {
    public id: string;
    public creationDate: string;
    public updateDate: string;
    public content: NoteContent;
}

export class NoteContent {
    public categoryIds: string[];
    public freeTags: string;
    public header: string;
    public text: string;

    public static createDefault(): NoteContent {
        return new NoteContent();
    }
}