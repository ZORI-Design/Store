const { nextui } = require("@nextui-org/react");

/** @type {import('tailwindcss').Config} */
export default {
    content: [
        "./src/**/*.{js,ts,jsx,tsx}",
        "./node_modules/@nextui-org/theme/dist/**/*.{js,ts,jsx,tsx}"
    ],
    theme: {
        extend: {},
        screens: {
            'sm': '640px',
            'md': '768px',
            'lg': '1024px',
            'xl': '1280px',
            '2xl': '1536px',
        }
    },
    darkMode: "class",
    plugins: [nextui({
        layout: {
            dividerWeight: "1px", // h-divider the default height applied to the divider component
            disabledOpacity: 0.5, // this value is applied as opacity-[value] when the component is disabled
            fontSize: {
                tiny: "8pt", // text-tiny
                small: "10.5pt", // text-small
                medium: "1rem", // text-medium
                large: "42pt", // text-large
            },
            lineHeight: {
                tiny: "1rem", // text-tiny
                small: "1.25rem", // text-small
                medium: "1.5rem", // text-medium
                large: "1.75rem", // text-large
            },
        }
    })],
}
