"use client"

export default function AppState({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {



    return (
        <>
        { children }
        </>
    );
}
